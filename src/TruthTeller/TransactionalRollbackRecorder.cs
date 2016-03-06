using System;
using System.Collections.Generic;

using M8.TruthTeller.Nodes;

namespace M8.TruthTeller {
  public class TransactionalRollbackRecorder : ITruthRecorder {
    private readonly ITruthRecorder _recorder;

    private readonly Stack<Node> _recordings = new Stack<Node>();

    public TransactionalRollbackRecorder( ITruthRecorder recorder ) {
      _recorder = recorder;
    }

    public IJustificationBuilder Justify( string consequentId ) {
      return _recorder.Justify( consequentId );
    }

    public void Assume( Node assumed ) {
      _recorder.Assume( assumed );
      _recordings.Push( assumed );
    }

    public void Premise( Node premised ) {
      _recorder.Premise( premised );
      _recordings.Push( premised );
    }

    public void Conjoin( params Justification[] justifications ) {
      _recorder.Conjoin( justifications );
    }

    IJustificationBuilder ITruthRecorder.Justify( Node consequent ) {
      return _recorder.Justify( consequent );
    }

    public void Retract( string node ) {
      _recorder.Retract( node );
    }

    public void Reset() {
      throw new InvalidOperationException( "Reset not allowed during Rollback transaction" );
    }

    public void Dispose() {
      foreach ( Node node in _recordings ) {
        _recorder.Retract( node.Id );
      }
    }

    public void Justify( Node justified ) {
      _recorder.Justify( justified );
      _recordings.Push( justified );
    }
  }
}