using System.Collections.Generic;
using System.Linq;

using M8.ATMS.Nodes;

namespace M8.ATMS {
  public class TruthTeller {
    private readonly TMS _tms;

    public TruthTeller( TMS tms ) {
      _tms = tms;
    }

    public bool IsOut( string nodeId ) {
      var node = GetNode( nodeId );
      return node.Label.Equals( Label.NoEnvironment );
    }

    public bool IsIn( string nodeId ) {
      var node = GetNode( nodeId );
      return
        node.Label.Any(
          e => !e.Equals( Label.EmptyEnvironment.Single() ) && e.IsProperSupersetOf( Label.EmptyEnvironment.Single() ) );
    }

    public bool IsTrue( string nodeId ) {
      var node = GetNode( nodeId );
      return node.Label.Any( e => e.Equals( Label.EmptyEnvironment.Single() ) );
    }

    public bool IsFalse( string nodeId ) {
      var contradiction = GetNode( CNode.Contradiction );
      return contradiction.Label.Any( e => IsSupported( nodeId, e ) );
    }

    public Node GetNode( string id ) {
      return _tms.GetNode( id );
    }

    public Environment GetEnvironment( string id ) {
      return _tms.GetEnvironment( id );
    }

    public List<string> GetAssumptions() {
      return _tms.GetAssumptions();
    }

    public List<Justification> GetJustificationFor( string consequent ) {
      return _tms.GetImmediateJustificationFor( consequent );
    }

    public bool IsSupported( string nodeId, Environment environment ) {
      var node = GetNode( nodeId );
      var label = node.Label;
      return label.Any( env => environment.Equals( env ) || environment.IsProperSupersetOf( env ) );
    }

    public IEnumerable<Node> GetNodes() {
      return _tms.GetNodes();
    }
  }
}