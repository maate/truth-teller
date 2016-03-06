using System;
using System.Linq;

using M8.TruthTeller.Nodes;

namespace M8.TruthTeller {
  public interface IJustificationBuilder {
    /// <summary>
    ///   Adds the specified
    ///   <param name="antecedents"></param>
    ///   to the justification.
    ///   Multiple antecedents are conjoined.
    /// </summary>
    /// <param name="antecedents"></param>
    void WithAntecedents( params string[] antecedents );

    /// <summary>
    ///   Adds a
    ///   <param name="description"></param>
    ///   to the justification.
    /// </summary>
    /// <param name="description"></param>
    void WithInformation( string description );
  }

  internal class JustificationBuilder : IJustificationBuilder {
    private readonly string _consequent;

    private readonly ITMS _tms;

    private Justification _justification;

    public JustificationBuilder( ITMS tms, Node consequent ) {
      _tms = tms;
      if ( !_tms.HasNode( consequent.Id ) ) {
        _tms.Add( consequent );
      }
      _consequent = consequent.Id;
    }

    public void WithAntecedents( params string[] antecedents ) {
      if ( antecedents == null || !antecedents.Any() ) {
        throw new ArgumentException( "Array cannot be empty", "antecedents" );
      }
      _justification = new Justification {
        Antecedents = new Conjunction<ANode>( antecedents.Select( a => new ANode( a ) ) ),
        Consequent = new CNode( _consequent )
      };
      _tms.Add( _justification );
    }

    public void WithInformation( string description ) {
      _justification.Informant = new Informant( description );
    }
  }
}