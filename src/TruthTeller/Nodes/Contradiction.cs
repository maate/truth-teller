namespace M8.TruthTeller.Nodes {
  public class Contradiction : CNode {
    internal Contradiction()
      : base( "⊥" ) {
    }

    public override int GetHashCode() {
      return GetType().GetHashCode();
    }

    public override bool Equals( object obj ) {
      return obj is Contradiction;
    }
  }
}