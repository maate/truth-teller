using System;
using System.Linq;
using System.Text;

namespace M8.ATMS.Nodes {
  /// <summary>
  ///   A justiﬁcation is a condition on a node and consists of three parts:
  ///   1. Consequent. The node of the datum that has been inferred by the inference engine.
  ///   2. Antecedents list. Nodes of the datums that are used by the inference engine to infer the datum of the consequent.
  ///   3. Informant. A comment that explains the inference in more detail.
  /// </summary>
  public class Justification {
    public Justification() {
      Antecedents = new Conjunction<ANode>();
    }

    /// <summary>
    ///   The node of the datum that has been inferred by the inference engine.
    /// </summary>
    public CNode Consequent { get; set; }

    /// <summary>
    ///   Nodes of the datums that are used by the inference engine to infer the datum of the consequent.
    /// </summary>
    public Conjunction<ANode> Antecedents { get; set; }

    /// <summary>
    ///   An optional comment that explains the inference in more detail.
    /// </summary>
    public Informant Informant { get; set; }

    public override string ToString() {
      var aStr = new StringBuilder();
      aStr.Append( "{" );
      if ( aStr.Length > 2 ) {
        aStr = aStr.Remove( aStr.Length - 2, 1 );
      }
      aStr.Append( "}" );
      return string.Format( "< {0}, {1} >", Consequent.Id, String.Join( ", ", Antecedents.Select( a => a.Id ) ) );
    }
  }
}