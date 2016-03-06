using System;

using M8.ATMS.Nodes;

namespace M8.ATMS {
  public interface ITruthRecorder : IDisposable {
    /// <summary>
    ///   Adds an assumption node to the TMS
    /// </summary>
    /// <param name="assumed"></param>
    void Assume( Node assumed );

    /// <summary>
    ///   Adds a premise node to the TMS
    /// </summary>
    /// <param name="premised"></param>
    void Premise( Node premised );

    /// <summary>
    ///   Conjoins two or more justifications
    /// </summary>
    /// <param name="justifications"></param>
    void Conjoin( params Justification[] justifications );

    /// <summary>
    ///   Records a justification. Records the consequent node as a part of the process.
    /// </summary>
    IJustificationBuilder Justify( Node consequent );

    /// <summary>
    ///   Records a justification. Expects an existing consequent node.
    /// </summary>
    IJustificationBuilder Justify( string consequentId );

    /// <summary>
    ///   Retracts a node from the TMS
    /// </summary>
    /// <param name="node"></param>
    void Retract( string node );

    /// <summary>
    ///   Resets the TMS. This will clear the full state of the TMS.
    /// </summary>
    void Reset();
  }
}