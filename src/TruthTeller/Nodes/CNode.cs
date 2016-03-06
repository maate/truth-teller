using System;

namespace M8.ATMS.Nodes {
  /// <summary>
  ///   Consequent Node. The node of the datum that has been inferred by the inference engine.
  /// </summary>
  public class CNode : Datum {
    private static Contradiction _contradictionInstance;
    private static readonly Object Mutex = new object();

    public CNode( string id ):base(id) {
    }

    public static Contradiction Contradiction {
      get {
        if ( _contradictionInstance == null ) {
          lock ( Mutex ) {
            if ( _contradictionInstance == null ) {
              _contradictionInstance = new Contradiction();
            }
          }
        }
        return _contradictionInstance;
      }
    }
  }
}