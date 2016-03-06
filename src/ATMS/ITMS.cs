using M8.ATMS.Nodes;

namespace M8.ATMS {
  public interface ITMS {
    void Add( Node node );

    void Add( Justification j );

    Node GetNode( string id );

    void Retract( string nodeId );

    void Conjoin( Justification[] justifications );

    void Reset();

    bool HasNode( string id );
  }
}