namespace M8.TruthTeller.Nodes {
  public class Node {
    public Node() {
      Label = Label.NoEnvironment;
    }

    public Node( string id ) {
      Id = id;
    }

    internal Node( string id, NodeType type )
      : this( id ) {
      NodeType = type;
    }

    internal NodeType NodeType { get; set; }

    public Label Label { get; internal set; }

    public string Id { get; set; }
  }
}