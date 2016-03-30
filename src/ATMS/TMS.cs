using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using M8.ATMS.Nodes;

using Environment = M8.ATMS.Nodes.Environment;

namespace M8.ATMS {
  public class TMS : ITMS {
    private readonly List<Justification> _justifications = new List<Justification>();

    private readonly Conjunction<Label> _nogoodSets = new Conjunction<Label>(); 

    private readonly Dictionary<string, Node> _nodes = new Dictionary<string, Node>();

    public void Add( Node node ) {
      switch ( node.NodeType ) {
        case NodeType.Justified:
          node.Label = Label.NoEnvironment;
          break;
        case NodeType.Premise:
          node.Label = Label.EmptyEnvironment;
          break;
        case NodeType.Assumption:
          node.Label = new Label { new Environment( node.Id ) };
          break;
      }
      _nodes.Add( node.Id, node );
    }

    public void Add( Justification j ) {
      if ( j.Consequent is Contradiction ) {
        EnsureContraditionNodeExists();
      }

      Node consequent = _nodes[j.Consequent.Id];
      UpdateConsequent( consequent );

      UpdateLabel( j, consequent );

      _justifications.Add( j );
    }

    private void UpdateLabel( Justification j, Node consequent, Func<ANode, bool> pred = null ) {
      Conjunction<Label> antecedentLabels = LookupAntecedentLabels( j.Antecedents );
      antecedentLabels = FilterByPredicate( pred, antecedentLabels );
      var label = new Label();

      label.Conjoin( antecedentLabels );

      FilterNogoodSets( j, label );

      consequent.Label.Disjoin( label );
      consequent.Label.RemoveSubsumed();
    }

    private static Conjunction<Label> FilterByPredicate( Func<ANode, bool> pred, Conjunction<Label> antecedentLabels ) {
      if ( pred != null ) {
        antecedentLabels = new Conjunction<Label>( antecedentLabels.Where( l => l.All( env => env.All( pred ) ) ) );
      }
      return antecedentLabels;
    }

    private void FilterNogoodSets( Justification j, Label label ) {
      if ( ShouldFilterNogoodSets( j ) ) {
        var nogood = GetNode( CNode.Contradiction.Id ).Label;
        label.FilterNogoodSets( nogood );
      }
    }

    private bool ShouldFilterNogoodSets( Justification j ) {
      return !( j.Consequent is Contradiction ) && _nodes.ContainsKey( CNode.Contradiction.Id );
    }

    private void EnsureContraditionNodeExists() {
      if ( !_nodes.ContainsKey( CNode.Contradiction.Id ) ) {
        _nodes.Add( CNode.Contradiction.Id, new Node( CNode.Contradiction.Id, NodeType.Contradiction ) );
      }
    }

    /// <summary>
    /// Retrieves the <see cref="Label"/> list from the antecedents mentioned by the <see cref="Justification"/>
    /// </summary>
    private Conjunction<Label> LookupAntecedentLabels( Conjunction<ANode> antecedents ) {
      var antecedentLabels = new Conjunction<Label>();

      foreach ( ANode aNode in antecedents ) {
        var label = _nodes[aNode.Id].Label;
        antecedentLabels.Add( label );
      }

      return antecedentLabels;
    }

    private static void UpdateConsequent( Node consequent ) {
      if ( consequent.Label == null ) {
        consequent.Label = Label.NoEnvironment;
      }
      consequent.NodeType = NodeType.Justified;
    }

    public Node GetNode( string id ) {
      if ( !_nodes.ContainsKey( id ) ) {
        throw new ArgumentException( string.Format( "TMS doesn't know node with id '{0}'", id ) );
      }
      return _nodes[id];
    }

    public bool HasNode( string id ) {
      return _nodes.ContainsKey( id );
    }

    internal Environment GetEnvironment( string id ) {
      Label label = _nodes[id].Label;
      if ( label == null ) {
        return null;
      }
      var env = new Environment();
      foreach ( Environment e in label ) {
        env.UnionWith( e );
      }
      return env;
    }

    internal List<string> GetAssumptions() {
      return _nodes.Where( n => n.Value.NodeType == NodeType.Assumption ).Select( n => n.Value.Id ).ToList();
    }

    public void Conjoin( Justification[] justifications ) {
      var consequent = justifications.First().Consequent;
      foreach ( Justification justification in justifications ) {
        if ( !_justifications.Contains( justification ) ) {
          throw new InvalidOperationException( "Record the justification before conjoining it" );
        }
        if ( !justification.Consequent.Equals( consequent ) ) {
          throw new InvalidOperationException( "Two conjoint justifications must share the same consequent" );
        }
      }
      var cnode = _nodes[consequent];
      cnode.Label = new Label();
      for(int i = 0; i < justifications.Length; i++) {
        Conjunction<Label> extendedJustification = LookupAntecedentLabels( justifications[i].Antecedents );
        cnode.Label.Conjoin( extendedJustification );
      }
      cnode.Label.RemoveSubsumed();
    }

    internal List<Justification> GetImmediateJustificationFor( string consequent ) {
      return _justifications.Where( j => j.Consequent.Id == consequent ).ToList();
    }

    public void Reset() {
      _nodes.Clear();
      _justifications.Clear();
      _nogoodSets.Clear();
    }

    public void Retract( string nodeId ) {
      var node = GetNode( nodeId );
      if ( node.NodeType == NodeType.Justified ) {
        throw new InvalidOperationException(
          "Cannot retract a justified node. Retract the assumption instead." );
      }
      if ( node.NodeType == NodeType.Premise ) {
        throw new InvalidOperationException( "Cannot retract a premise. Premises must always hold." );
      }
      if ( node.NodeType == NodeType.Contradiction ) {
        throw new InvalidOperationException(
          "Cannot retract a contradiction. Contradicionts are justified nodes. Retract the assumption that causes the contradiction instead." );
      }
      Update( nodeId, nodeId, true );
      _nodes.Remove( nodeId );
    }

    private void Update( string nodeId, string retractBase, bool retract = false ) {
      var justifications = ImmediatelyJustifies( nodeId );
      var cnodes = justifications.Select( j => j.Consequent );

      foreach ( CNode cnode in cnodes.ToList() ) {
        var justifiedBy = GetImmediateJustificationFor( cnode );
        var node = GetNode( cnode );
        node.Label = new Label();
        var markedForDeletion = new List<Justification>();
        foreach ( Justification justification in justifiedBy ) {
          if ( retract ) {
            FilterRetractedAntecedents( nodeId, justification );
          }
          UpdateLabel( justification, node, n => n != retractBase );
          if ( !justification.Antecedents.Any() ) {
            markedForDeletion.Add( justification );
          }
        }
        Update( cnode, retractBase, node.Label.Equals( Label.NoEnvironment ) && justifiedBy.All( j => j.Antecedents.All( i => i.Id == nodeId ) ) );

        foreach ( var justification in markedForDeletion ) {
          _justifications.Remove( justification );
        }

        if ( !GetImmediateJustificationFor( cnode ).Any() ) {
          _nodes.Remove( cnode );
        }
      }
    }

    private static void FilterRetractedAntecedents( string nodeId, Justification justification ) {
      justification.Antecedents =
        new Conjunction<ANode>(
          justification.Antecedents.Except(
            justification.Antecedents.Where( a => a.Id.Equals( nodeId, StringComparison.InvariantCultureIgnoreCase ) ) ) );
    }

    private IEnumerable<Justification> ImmediatelyJustifies( string nodeId ) {
      return _justifications.Where(
        j => j.Antecedents.Any( a => a.Id.Equals( nodeId, StringComparison.InvariantCultureIgnoreCase ) ) );
    }

    internal IEnumerable<Node> GetNodes() {
      return new ReadOnlyCollection<Node>( _nodes.Values.ToList() );
    }
  }
}