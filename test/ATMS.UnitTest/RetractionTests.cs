using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using M8.ATMS.Nodes;

using NUnit.Framework;

namespace M8.ATMS.UnitTest {
  [TestFixture]
  public class RetractionTests {
    private TruthRecorder _recorder;
    private TruthTeller _informant;

    [SetUp]
    public void SetUp() {
      var tms = new TMS();
      _recorder = new TruthRecorder( tms );
      _informant = new TruthTeller( tms );
    }

    [Test]
    public void WhenRetractingSingletonAssumption_ThenJustificationIsRetracted() {
      // arrange
      _recorder.Assume( new Node( "A" ) );
      _recorder.Justify( new Node( "P" ) )
               .WithAntecedents( "A" );

      // act
      _recorder.Retract( "A" );

      // assert
      Assert.That( _informant.GetNodes(), Is.Empty );
    }

    [Test]
    public void WhenRetractingAllAssumptions_ThenJustificationIsRetracted() {
      // arrange
      _recorder.Assume( new Node( "A" ) );
      _recorder.Assume( new Node( "B" ) );
      _recorder.Justify( new Node( "P" ) )
               .WithAntecedents( "A", "B" );

      // act
      _recorder.Retract( "A" );
      _recorder.Retract( "B" );

      // assert
      Assert.That( _informant.GetNodes(), Is.Empty );
    }

    [Test]
    public void WhenRetractingOneOfTwoAssumptions_ThenJustificationIsNotRetracted() {
      // arrange
      _recorder.Assume( new Node( "A" ) );
      _recorder.Assume( new Node( "B" ) );
      _recorder.Justify( new Node( "P" ) )
               .WithAntecedents( "A", "B" );

      // act
      _recorder.Retract( "A" );

      // assert
      Assert.That( _informant.GetNodes(), Is.Not.Empty );
    }

    [Test]
    public void WhenRetractingSingletonAssumptions_ThenJustificationsAreRetractedTransitively() {
      // arrange
      _recorder.Assume( new Node( "A" ) );

      _recorder.Justify( new Node( "P" ) )
               .WithAntecedents( "A" );

      _recorder.Justify( new Node( "Q" ) )
               .WithAntecedents( "P" );

      // act
      _recorder.Retract( "A" );

      // assert
      Assert.That( _informant.GetNodes(), Is.Empty );
    }

    [Test]
    /*
     Tests retraction of A in the scenario:

     A --> P -->  -----
                 |  Q  |
     B -------->  -----

     After retracting A, it is expected that Q is still justified by B
     */
    public void WhenRetractingSingletonAssumption_IfJustificationIsJustifiedByOtherSingleton_ThenJustificationsAreNotRetractedTransitively() {
      // arrange
      _recorder.Assume( new Node( "B" ) );

      _recorder.Justify( new Node( "Q" ) )
               .WithAntecedents( "B" );

      _recorder.Assume( new Node( "A" ) );

      _recorder.Justify( new Node( "P" ) )
               .WithAntecedents( "A" );

      _recorder.Justify( new Node( "Q" ) )
               .WithAntecedents( "P" );


      // act
      _recorder.Retract( "A" );

      // assert
      Assert.That( _informant.GetJustificationFor( "Q" ).Single().Antecedents.Single().Id, Is.EqualTo( "B" ) );
    }

    [Test]
    /*
     Tests retraction of A in the scenario:

     A --> P -->  -----
           R <-- |  Q  |
             ---> -----

     After retracting A, it is expected that Q is still justified by B
     */
    public void WhenRetractingJustificationForCircularDependency_ThenJustificationsAreRetractedTransitively() {
      // arrange
      _recorder.Assume( new Node( "A" ) );

      _recorder.Justify( new Node( "P" ) )
               .WithAntecedents( "A" );

      _recorder.Justify( new Node( "Q" ) )
               .WithAntecedents( "P" );

      _recorder.Justify( new Node( "R" ) )
               .WithAntecedents( "Q" );

      _recorder.Justify( new Node( "Q" ) )
               .WithAntecedents( "R" );

      // act
      _recorder.Retract( "A" );

      // assert
      Assert.That( _informant.GetNodes(), Is.Empty );
    }

  }
}
