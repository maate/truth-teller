namespace M8.ATMS.Nodes {
  /// <summary>
  ///   Antecedent Node. Node of a datum that are used by the inference engine to infer the datum of the <see cref="CNode" />
  /// </summary>
  public class ANode : Datum {
    public ANode( string id )
      : base( id ) {
    }
  }
}