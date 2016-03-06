using System.Linq;

using M8.ATMS.Nodes;

using NUnit.Framework;

namespace M8.ATMS.UnitTest {
  [TestFixture]
  internal class NogoodSetTests : TestBase {
    public void InitNoGoodSetWithSingleANode() {
      Recorder.Reset();

      Recorder.Assume( new Node( "Q" ) );
      Recorder.Assume( new Node( "R" ) );
      Recorder.Assume( new Node( "S" ) );
      Recorder.Assume( new Node( "T" ) );
      Recorder.Contradict( "R" );
      Recorder.Justify( "z" ).WithAntecedents( "R", "S" );
      Recorder.Justify( "z" ).WithAntecedents( "S", "T" );
    }

    public void InitNoGoodSetWithTwoANodes() {
      Recorder.Reset();

      Recorder.Assume( new Node( "Q" ) );
      Recorder.Assume( new Node( "R" ) );
      Recorder.Assume( new Node( "S" ) );
      Recorder.Assume( new Node( "T" ) );
      Recorder.Contradict( "R", "S" );
      Recorder.Justify( "z" ).WithAntecedents( "R", "S" );
      Recorder.Justify( "z" ).WithAntecedents( "S", "T" );
    }

    public void InitInterleavedNoGoodSetWithTwoANodes() {
      Recorder.Reset();

      Recorder.Assume( new Node( "Q" ) );
      Recorder.Assume( new Node( "R" ) );
      Recorder.Assume( new Node( "S" ) );
      Recorder.Assume( new Node( "T" ) );
      Recorder.Contradict( "R", "Q" );
      Recorder.Justify( new Node( "z" ) ).WithAntecedents( "R", "S" );
      Recorder.Justify( new Node( "z" ) ).WithAntecedents( "S", "T" );
    }

    [Test]
    public void ContradictionEnvironment() {
      InitNoGoodSetWithSingleANode();
      Environment env = Informant.GetEnvironment( CNode.Contradiction.Id );
      Assert.That( env.Count(), Is.EqualTo( 1 ) );
      Assert.That( env.ContainsId( "R" ) );
    }

    [Test]
    public void WhenEnvironmentSuperseedesNogoodSet_ThenLabelExcludesNogoodSet() {
      InitNoGoodSetWithSingleANode();
      AssertLabel( "z", "{{S,T}}" );
    }

    [Test]
    public void WhenEnvironmentEqualsNogoodSet_ThenLabelExcludesNogoodSet() {
      InitNoGoodSetWithTwoANodes();
      AssertLabel( "z", "{{S,T}}" );
    }

    [Test]
    public void WhenEnvironmentDoesNotKnowOfNogoodSet_ThenLabelDoesNotExcludeNogoodSet() {
      InitInterleavedNoGoodSetWithTwoANodes();
      AssertLabel( "z", "{{R,S},{S,T}}" );
    }
  }
}