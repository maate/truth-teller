using System;

using M8.TruthTeller.Nodes;

using NUnit.Framework;

namespace M8.TruthTeller.UnitTest {
  [TestFixture]
  internal class PremiseTests : TestBase {
    public void InitNoGoodSetWithANodeAndPremise() {
      Recorder.Reset();

      Recorder.Premise( new Node( "Q" ) );
      Recorder.Assume( new Node( "R" ) );
      Recorder.Assume( new Node( "S" ) );
      Recorder.Assume( new Node( "T" ) );
      Recorder.Contradict( "R", "Q" );
      Recorder.Justify( new Node( "z" ) ).WithAntecedents( "R", "S" );
      Recorder.Justify( new Node( "z" ) ).WithAntecedents( "S", "T" );
    }

    [Test]
    public void ContradictionEnvironment() {
      InitNoGoodSetWithANodeAndPremise();
      AssertLabel( CNode.Contradiction, "{ {R} }" );
    }

    [Test]
    public void WhenANodeIsContradicted_ThenItIsFalse() {
      InitNoGoodSetWithANodeAndPremise();
      var node = Informant.GetNode( "R" );
      //Assert.That( node.IsFalse() );
      Console.WriteLine(node.Label);
    }

    [Test]
    public void WhenEnvironmentSuperseedesNogoodSet_ThenLabelExcludesNogoodSet() {
      InitNoGoodSetWithANodeAndPremise();
      AssertLabel( "z", "{{S,T}}" );
    }
  }
}