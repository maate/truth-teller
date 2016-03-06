using System.Collections.Generic;
using System.Linq;

namespace M8.ATMS.Nodes {
  /// <summary>
  ///   A set of assumption nodes.
  /// </summary>
  public class Environment : Conjunction<ANode> {
    public Environment() {
    }

    public Environment( IEnumerable<ANode> self )
      : base( self ) {
    }

    public Environment( params string[] ids )
      : base( ids.Select( s => new ANode( s ) ) ) {
    }

    public bool ContainsId( string id ) {
      return Values.Contains( new ANode( id ) );
    }

    public new Environment Clone() {
      var conj = new Environment();
      foreach ( var item in Values ) {
        conj.Add( item );
      }
      return conj;
    }

    /// <summary>
    ///   Adds an ANode to the environment by ID reference
    /// </summary>
    /// <param name="aNodeId"></param>
    public void AddId( string aNodeId ) {
      Add( new ANode( aNodeId ) );
    }

  }
}