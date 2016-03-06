using System;
using System.Linq;

using M8.TruthTeller.Nodes;

namespace M8.TruthTeller {
  public class TruthRecorder : ITruthRecorder {
    private readonly ITMS _tms;

    public TruthRecorder()
      : this( new TMS() ) {
    }

    public TruthRecorder( ITMS tms ) {
      _tms = tms;
    }

    public void Assume( Node assumption ) {
      if ( assumption == null ) {
        throw new ArgumentNullException( "assumption" );
      }
      assumption.NodeType = NodeType.Assumption;
      _tms.Add( assumption );
    }

    public void Premise( Node premise ) {
      if ( premise == null ) {
        throw new ArgumentNullException( "premise" );
      }
      premise.NodeType = NodeType.Premise;
      _tms.Add( premise  );
    }

    public void Contradict( params string[] antecedents ) {
      if ( antecedents == null || !antecedents.Any() ) {
        throw new ArgumentException( "Conjunction cannot be empty", "antecedents" );
      }

      _tms.Add(
        new Justification {
          Antecedents = new Conjunction<ANode>( antecedents.Select( a => new ANode( a ) ) ),
          Consequent = CNode.Contradiction
        } );
    }

    /// <summary>
    /// Conjoins two or more justifications
    /// </summary>
    /// <param name="justifications"></param>
    public void Conjoin( params Justification[] justifications ) {
      if ( !justifications.Any() ) {
        return;
      }
      _tms.Conjoin( justifications );
    }

    public IJustificationBuilder Justify( string consequentId ) {
      if ( string.IsNullOrEmpty( consequentId ) ) {
        throw new ArgumentNullException( consequentId );
      }

      var node = _tms.HasNode( consequentId ) ? _tms.GetNode( consequentId ) : new Node( consequentId, NodeType.Justified );
      return Justify( node );
    }

    /// <summary>
    /// Records a justification
    /// </summary>
    public IJustificationBuilder Justify( Node consequent ) {
      return new JustificationBuilder( _tms, consequent );
    }

    public void Retract( string node ) {
      _tms.Retract( node );
    }

    public void Reset() {
      _tms.Reset();
    }

    public void Dispose() {
    }
  }
}