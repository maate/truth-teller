namespace M8.ATMS.Nodes {
  public enum NodeType {
    /// <summary>
    ///   The TMS logically infers the node.
    /// </summary>
    Justified,

    /// <summary>
    ///   The inference engine indicates that the associated datum is always true
    /// </summary>
    Premise,

    /// <summary>
    ///   The inference engine indicates that the associated datum is always false
    /// </summary>
    Contradiction,

    /// <summary>
    ///   The inference engine indicates that the associated datum is true unless stated otherwise.
    /// </summary>
    Assumption,
  }
}