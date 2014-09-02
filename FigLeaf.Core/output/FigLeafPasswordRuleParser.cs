//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 3.5
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// $ANTLR 3.5 D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g 2014-09-02 17:26:51

// The variable 'variable' is assigned but its value is never used.
#pragma warning disable 168, 219
// Unreachable code detected.
#pragma warning disable 162
// Missing XML comment for publicly visible type or member 'Type_or_Member'
#pragma warning disable 1591


	using FigLeaf.Core;
	using FigLeaf.Core.PasswordRules;


using System.Collections.Generic;
using Antlr.Runtime;
using Antlr.Runtime.Misc;
using ConditionalAttribute = System.Diagnostics.ConditionalAttribute;

namespace  FigLeaf.Core.PasswordRules 
{
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "3.5")]
[System.CLSCompliant(false)]
public partial class FigLeafPasswordRuleParser : Antlr.Runtime.Parser
{
	internal static readonly string[] tokenNames = new string[] {
		"<invalid>", "<EOR>", "<DOWN>", "<UP>", "CLOSEBRACKET", "COMMA", "DIGIT", "OPENBRACKET", "SPACE", "'Add'", "'Digits'", "'FileExtension'", "'FileName'", "'Left'", "'Len'", "'Lower'", "'Password'", "'RemoveFileExtension'", "'Reverse'", "'Right'", "'Upper'"
	};
	public const int EOF=-1;
	public const int T__9=9;
	public const int T__10=10;
	public const int T__11=11;
	public const int T__12=12;
	public const int T__13=13;
	public const int T__14=14;
	public const int T__15=15;
	public const int T__16=16;
	public const int T__17=17;
	public const int T__18=18;
	public const int T__19=19;
	public const int T__20=20;
	public const int CLOSEBRACKET=4;
	public const int COMMA=5;
	public const int DIGIT=6;
	public const int OPENBRACKET=7;
	public const int SPACE=8;

	#if ANTLR_DEBUG
		private static readonly bool[] decisionCanBacktrack =
			new bool[]
			{
				false, // invalid decision
				false, false, false, false, false
			};
	#else
		private static readonly bool[] decisionCanBacktrack = new bool[0];
	#endif
	public FigLeafPasswordRuleParser(ITokenStream input)
		: this(input, new RecognizerSharedState())
	{
	}
	public FigLeafPasswordRuleParser(ITokenStream input, RecognizerSharedState state)
		: base(input, state)
	{
		OnCreated();
	}

	public override string[] TokenNames { get { return FigLeafPasswordRuleParser.tokenNames; } }
	public override string GrammarFileName { get { return "D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g"; } }


	[Conditional("ANTLR_TRACE")]
	protected virtual void OnCreated() {}
	[Conditional("ANTLR_TRACE")]
	protected virtual void EnterRule(string ruleName, int ruleIndex) {}
	[Conditional("ANTLR_TRACE")]
	protected virtual void LeaveRule(string ruleName, int ruleIndex) {}

	#region Rules

	[Conditional("ANTLR_TRACE")]
	protected virtual void EnterRule_baseArgument() {}
	[Conditional("ANTLR_TRACE")]
	protected virtual void LeaveRule_baseArgument() {}
	// $ANTLR start "baseArgument"
	// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:34:1: baseArgument[string parseFileName, string parsePassword] returns [string value] : ( password | fileName );
	[GrammarRule("baseArgument")]
	private string baseArgument(string parseFileName, string parsePassword)
	{
		EnterRule_baseArgument();
		EnterRule("baseArgument", 1);
		TraceIn("baseArgument", 1);
	    string value = default(string);


		try { DebugEnterRule(GrammarFileName, "baseArgument");
		DebugLocation(34, 1);
		try
		{
			// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:35:2: ( password | fileName )
			int alt1=2;
			try { DebugEnterDecision(1, decisionCanBacktrack[1]);
			int LA1_1 = input.LA(1);

			if ((LA1_1==16))
			{
				alt1 = 1;
			}
			else if ((LA1_1==12))
			{
				alt1 = 2;
			}
			else
			{
				NoViableAltException nvae = new NoViableAltException("", 1, 0, input, 1);
				DebugRecognitionException(nvae);
				throw nvae;
			}
			} finally { DebugExitDecision(1); }
			switch (alt1)
			{
			case 1:
				DebugEnterAlt(1);
				// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:35:4: password
				{
				DebugLocation(35, 4);
				PushFollow(Follow._password_in_baseArgument67);
				password();
				PopFollow();

				DebugLocation(36, 2);

						value = parsePassword;
					

				}
				break;
			case 2:
				DebugEnterAlt(2);
				// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:39:4: fileName
				{
				DebugLocation(39, 4);
				PushFollow(Follow._fileName_in_baseArgument75);
				fileName();
				PopFollow();

				DebugLocation(40, 2);

						value = parseFileName;
					

				}
				break;

			}
		}

			catch (RecognitionException e)
			{
				throw;
			}

		finally
		{
			TraceOut("baseArgument", 1);
			LeaveRule("baseArgument", 1);
			LeaveRule_baseArgument();
	    }
	 	DebugLocation(43, 1);
		} finally { DebugExitRule(GrammarFileName, "baseArgument"); }
		return value;

	}
	// $ANTLR end "baseArgument"


	[Conditional("ANTLR_TRACE")]
	protected virtual void EnterRule_functionArgument() {}
	[Conditional("ANTLR_TRACE")]
	protected virtual void LeaveRule_functionArgument() {}
	// $ANTLR start "functionArgument"
	// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:45:8: public functionArgument[string parseFileName, string parsePassword] returns [string value] : (ba= baseArgument[parseFileName, parsePassword] |n= number |f= function[parseFileName, parsePassword] );
	[GrammarRule("functionArgument")]
	public string functionArgument(string parseFileName, string parsePassword)
	{
		EnterRule_functionArgument();
		EnterRule("functionArgument", 2);
		TraceIn("functionArgument", 2);
	    string value = default(string);


	    string ba = default(string);
	    ParserRuleReturnScope<IToken> n = default(ParserRuleReturnScope<IToken>);
	    string f = default(string);

		try { DebugEnterRule(GrammarFileName, "functionArgument");
		DebugLocation(45, 1);
		try
		{
			// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:46:2: (ba= baseArgument[parseFileName, parsePassword] |n= number |f= function[parseFileName, parsePassword] )
			int alt2=3;
			try { DebugEnterDecision(2, decisionCanBacktrack[2]);
			switch (input.LA(1))
			{
			case 12:
			case 16:
				{
				alt2 = 1;
				}
				break;
			case DIGIT:
				{
				alt2 = 2;
				}
				break;
			case 9:
			case 10:
			case 11:
			case 13:
			case 14:
			case 15:
			case 17:
			case 18:
			case 19:
			case 20:
				{
				alt2 = 3;
				}
				break;
			default:
				{
					NoViableAltException nvae = new NoViableAltException("", 2, 0, input, 1);
					DebugRecognitionException(nvae);
					throw nvae;
				}
			}

			} finally { DebugExitDecision(2); }
			switch (alt2)
			{
			case 1:
				DebugEnterAlt(1);
				// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:46:4: ba= baseArgument[parseFileName, parsePassword]
				{
				DebugLocation(46, 7);
				PushFollow(Follow._baseArgument_in_functionArgument99);
				ba=baseArgument(parseFileName, parsePassword);
				PopFollow();

				DebugLocation(47, 2);

						value = ba;
					

				}
				break;
			case 2:
				DebugEnterAlt(2);
				// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:50:4: n= number
				{
				DebugLocation(50, 6);
				PushFollow(Follow._number_in_functionArgument112);
				n=number();
				PopFollow();

				DebugLocation(51, 2);

						value = (n!=null?input.ToString(n.Start,n.Stop):default(string));
					

				}
				break;
			case 3:
				DebugEnterAlt(3);
				// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:54:4: f= function[parseFileName, parsePassword]
				{
				DebugLocation(54, 6);
				PushFollow(Follow._function_in_functionArgument124);
				f=function(parseFileName, parsePassword);
				PopFollow();

				DebugLocation(55, 2);

						value = f;
					

				}
				break;

			}
		}

			catch (RecognitionException e)
			{
				throw;
			}

		finally
		{
			TraceOut("functionArgument", 2);
			LeaveRule("functionArgument", 2);
			LeaveRule_functionArgument();
	    }
	 	DebugLocation(58, 1);
		} finally { DebugExitRule(GrammarFileName, "functionArgument"); }
		return value;

	}
	// $ANTLR end "functionArgument"


	[Conditional("ANTLR_TRACE")]
	protected virtual void EnterRule_function() {}
	[Conditional("ANTLR_TRACE")]
	protected virtual void LeaveRule_function() {}
	// $ANTLR start "function"
	// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:60:1: function[string parseFileName, string parsePassword] returns [string value] : (uf= unaryFunction[parseFileName, parsePassword] |bf= binaryFunction[parseFileName, parsePassword] );
	[GrammarRule("function")]
	private string function(string parseFileName, string parsePassword)
	{
		EnterRule_function();
		EnterRule("function", 3);
		TraceIn("function", 3);
	    string value = default(string);


	    string uf = default(string);
	    string bf = default(string);

		try { DebugEnterRule(GrammarFileName, "function");
		DebugLocation(60, 1);
		try
		{
			// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:61:2: (uf= unaryFunction[parseFileName, parsePassword] |bf= binaryFunction[parseFileName, parsePassword] )
			int alt3=2;
			try { DebugEnterDecision(3, decisionCanBacktrack[3]);
			int LA3_1 = input.LA(1);

			if (((LA3_1>=10 && LA3_1<=11)||(LA3_1>=14 && LA3_1<=15)||(LA3_1>=17 && LA3_1<=18)||LA3_1==20))
			{
				alt3 = 1;
			}
			else if ((LA3_1==9||LA3_1==13||LA3_1==19))
			{
				alt3 = 2;
			}
			else
			{
				NoViableAltException nvae = new NoViableAltException("", 3, 0, input, 1);
				DebugRecognitionException(nvae);
				throw nvae;
			}
			} finally { DebugExitDecision(3); }
			switch (alt3)
			{
			case 1:
				DebugEnterAlt(1);
				// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:61:4: uf= unaryFunction[parseFileName, parsePassword]
				{
				DebugLocation(61, 7);
				PushFollow(Follow._unaryFunction_in_function147);
				uf=unaryFunction(parseFileName, parsePassword);
				PopFollow();

				DebugLocation(62, 2);

						value = uf;
					

				}
				break;
			case 2:
				DebugEnterAlt(2);
				// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:65:4: bf= binaryFunction[parseFileName, parsePassword]
				{
				DebugLocation(65, 7);
				PushFollow(Follow._binaryFunction_in_function160);
				bf=binaryFunction(parseFileName, parsePassword);
				PopFollow();

				DebugLocation(66, 2);

						value = bf;
					

				}
				break;

			}
		}

			catch (RecognitionException e)
			{
				throw;
			}

		finally
		{
			TraceOut("function", 3);
			LeaveRule("function", 3);
			LeaveRule_function();
	    }
	 	DebugLocation(69, 1);
		} finally { DebugExitRule(GrammarFileName, "function"); }
		return value;

	}
	// $ANTLR end "function"


	[Conditional("ANTLR_TRACE")]
	protected virtual void EnterRule_binaryFunction() {}
	[Conditional("ANTLR_TRACE")]
	protected virtual void LeaveRule_binaryFunction() {}
	// $ANTLR start "binaryFunction"
	// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:71:1: binaryFunction[string parseFileName, string parsePassword] returns [string value] : func= binaryFunctionName OPENBRACKET arg1= functionArgument[parseFileName, parsePassword] COMMA ( SPACE )* arg2= functionArgument[parseFileName, parsePassword] CLOSEBRACKET ;
	[GrammarRule("binaryFunction")]
	private string binaryFunction(string parseFileName, string parsePassword)
	{
		EnterRule_binaryFunction();
		EnterRule("binaryFunction", 4);
		TraceIn("binaryFunction", 4);
	    string value = default(string);


	    ParserRuleReturnScope<IToken> func = default(ParserRuleReturnScope<IToken>);
	    string arg1 = default(string);
	    string arg2 = default(string);

		try { DebugEnterRule(GrammarFileName, "binaryFunction");
		DebugLocation(71, 1);
		try
		{
			// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:72:2: (func= binaryFunctionName OPENBRACKET arg1= functionArgument[parseFileName, parsePassword] COMMA ( SPACE )* arg2= functionArgument[parseFileName, parsePassword] CLOSEBRACKET )
			DebugEnterAlt(1);
			// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:72:4: func= binaryFunctionName OPENBRACKET arg1= functionArgument[parseFileName, parsePassword] COMMA ( SPACE )* arg2= functionArgument[parseFileName, parsePassword] CLOSEBRACKET
			{
			DebugLocation(72, 9);
			PushFollow(Follow._binaryFunctionName_in_binaryFunction183);
			func=binaryFunctionName();
			PopFollow();

			DebugLocation(72, 30);
			Match(input,OPENBRACKET,Follow._OPENBRACKET_in_binaryFunction185); 
			DebugLocation(72, 47);
			PushFollow(Follow._functionArgument_in_binaryFunction191);
			arg1=functionArgument(parseFileName, parsePassword);
			PopFollow();

			DebugLocation(72, 96);
			Match(input,COMMA,Follow._COMMA_in_binaryFunction194); 
			DebugLocation(72, 102);
			// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:72:102: ( SPACE )*
			try { DebugEnterSubRule(4);
			while (true)
			{
				int alt4=2;
				try { DebugEnterDecision(4, decisionCanBacktrack[4]);
				int LA4_1 = input.LA(1);

				if ((LA4_1==SPACE))
				{
					alt4 = 1;
				}


				} finally { DebugExitDecision(4); }
				switch ( alt4 )
				{
				case 1:
					DebugEnterAlt(1);
					// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:72:102: SPACE
					{
					DebugLocation(72, 102);
					Match(input,SPACE,Follow._SPACE_in_binaryFunction196); 

					}
					break;

				default:
					goto loop4;
				}
			}

			loop4:
				;

			} finally { DebugExitSubRule(4); }

			DebugLocation(72, 114);
			PushFollow(Follow._functionArgument_in_binaryFunction203);
			arg2=functionArgument(parseFileName, parsePassword);
			PopFollow();

			DebugLocation(72, 163);
			Match(input,CLOSEBRACKET,Follow._CLOSEBRACKET_in_binaryFunction206); 
			DebugLocation(73, 2);

					System.Func<string, string, string> function = Functions.GetBinaryFunction((func!=null?input.ToString(func.Start,func.Stop):default(string)));
					value = function(arg1, arg2);
				

			}

		}

			catch (RecognitionException e)
			{
				throw;
			}

		finally
		{
			TraceOut("binaryFunction", 4);
			LeaveRule("binaryFunction", 4);
			LeaveRule_binaryFunction();
	    }
	 	DebugLocation(77, 1);
		} finally { DebugExitRule(GrammarFileName, "binaryFunction"); }
		return value;

	}
	// $ANTLR end "binaryFunction"


	[Conditional("ANTLR_TRACE")]
	protected virtual void EnterRule_unaryFunction() {}
	[Conditional("ANTLR_TRACE")]
	protected virtual void LeaveRule_unaryFunction() {}
	// $ANTLR start "unaryFunction"
	// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:79:1: unaryFunction[string parseFileName, string parsePassword] returns [string value] : func= unaryFunctionName OPENBRACKET arg= functionArgument[parseFileName, parsePassword] CLOSEBRACKET ;
	[GrammarRule("unaryFunction")]
	private string unaryFunction(string parseFileName, string parsePassword)
	{
		EnterRule_unaryFunction();
		EnterRule("unaryFunction", 5);
		TraceIn("unaryFunction", 5);
	    string value = default(string);


	    ParserRuleReturnScope<IToken> func = default(ParserRuleReturnScope<IToken>);
	    string arg = default(string);

		try { DebugEnterRule(GrammarFileName, "unaryFunction");
		DebugLocation(79, 1);
		try
		{
			// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:80:2: (func= unaryFunctionName OPENBRACKET arg= functionArgument[parseFileName, parsePassword] CLOSEBRACKET )
			DebugEnterAlt(1);
			// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:80:4: func= unaryFunctionName OPENBRACKET arg= functionArgument[parseFileName, parsePassword] CLOSEBRACKET
			{
			DebugLocation(80, 9);
			PushFollow(Follow._unaryFunctionName_in_unaryFunction228);
			func=unaryFunctionName();
			PopFollow();

			DebugLocation(80, 29);
			Match(input,OPENBRACKET,Follow._OPENBRACKET_in_unaryFunction230); 
			DebugLocation(80, 45);
			PushFollow(Follow._functionArgument_in_unaryFunction236);
			arg=functionArgument(parseFileName, parsePassword);
			PopFollow();

			DebugLocation(80, 94);
			Match(input,CLOSEBRACKET,Follow._CLOSEBRACKET_in_unaryFunction239); 
			DebugLocation(81, 2);

					System.Func<string, string> function = Functions.GetUnaryFunction((func!=null?input.ToString(func.Start,func.Stop):default(string)));
					value = function(arg);
				

			}

		}

			catch (RecognitionException e)
			{
				throw;
			}

		finally
		{
			TraceOut("unaryFunction", 5);
			LeaveRule("unaryFunction", 5);
			LeaveRule_unaryFunction();
	    }
	 	DebugLocation(85, 1);
		} finally { DebugExitRule(GrammarFileName, "unaryFunction"); }
		return value;

	}
	// $ANTLR end "unaryFunction"


	[Conditional("ANTLR_TRACE")]
	protected virtual void EnterRule_unaryFunctionName() {}
	[Conditional("ANTLR_TRACE")]
	protected virtual void LeaveRule_unaryFunctionName() {}
	// $ANTLR start "unaryFunctionName"
	// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:87:1: unaryFunctionName : ( 'Reverse' | 'Upper' | 'Lower' | 'Digits' | 'RemoveFileExtension' | 'FileExtension' | 'Len' );
	[GrammarRule("unaryFunctionName")]
	private ParserRuleReturnScope<IToken> unaryFunctionName()
	{
		EnterRule_unaryFunctionName();
		EnterRule("unaryFunctionName", 6);
		TraceIn("unaryFunctionName", 6);
	    ParserRuleReturnScope<IToken> retval = new ParserRuleReturnScope<IToken>();
	    retval.Start = (IToken)input.LT(1);

		try { DebugEnterRule(GrammarFileName, "unaryFunctionName");
		DebugLocation(87, 8);
		try
		{
			// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:88:2: ( 'Reverse' | 'Upper' | 'Lower' | 'Digits' | 'RemoveFileExtension' | 'FileExtension' | 'Len' )
			DebugEnterAlt(1);
			// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:
			{
			DebugLocation(88, 2);
			if ((input.LA(1)>=10 && input.LA(1)<=11)||(input.LA(1)>=14 && input.LA(1)<=15)||(input.LA(1)>=17 && input.LA(1)<=18)||input.LA(1)==20)
			{
				input.Consume();
				state.errorRecovery=false;
			}
			else
			{
				MismatchedSetException mse = new MismatchedSetException(null,input);
				DebugRecognitionException(mse);
				throw mse;
			}


			}

			retval.Stop = (IToken)input.LT(-1);

		}

			catch (RecognitionException e)
			{
				throw;
			}

		finally
		{
			TraceOut("unaryFunctionName", 6);
			LeaveRule("unaryFunctionName", 6);
			LeaveRule_unaryFunctionName();
	    }
	 	DebugLocation(94, 8);
		} finally { DebugExitRule(GrammarFileName, "unaryFunctionName"); }
		return retval;

	}
	// $ANTLR end "unaryFunctionName"


	[Conditional("ANTLR_TRACE")]
	protected virtual void EnterRule_binaryFunctionName() {}
	[Conditional("ANTLR_TRACE")]
	protected virtual void LeaveRule_binaryFunctionName() {}
	// $ANTLR start "binaryFunctionName"
	// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:96:1: binaryFunctionName : ( 'Add' | 'Left' | 'Right' );
	[GrammarRule("binaryFunctionName")]
	private ParserRuleReturnScope<IToken> binaryFunctionName()
	{
		EnterRule_binaryFunctionName();
		EnterRule("binaryFunctionName", 7);
		TraceIn("binaryFunctionName", 7);
	    ParserRuleReturnScope<IToken> retval = new ParserRuleReturnScope<IToken>();
	    retval.Start = (IToken)input.LT(1);

		try { DebugEnterRule(GrammarFileName, "binaryFunctionName");
		DebugLocation(96, 10);
		try
		{
			// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:97:2: ( 'Add' | 'Left' | 'Right' )
			DebugEnterAlt(1);
			// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:
			{
			DebugLocation(97, 2);
			if (input.LA(1)==9||input.LA(1)==13||input.LA(1)==19)
			{
				input.Consume();
				state.errorRecovery=false;
			}
			else
			{
				MismatchedSetException mse = new MismatchedSetException(null,input);
				DebugRecognitionException(mse);
				throw mse;
			}


			}

			retval.Stop = (IToken)input.LT(-1);

		}

			catch (RecognitionException e)
			{
				throw;
			}

		finally
		{
			TraceOut("binaryFunctionName", 7);
			LeaveRule("binaryFunctionName", 7);
			LeaveRule_binaryFunctionName();
	    }
	 	DebugLocation(99, 10);
		} finally { DebugExitRule(GrammarFileName, "binaryFunctionName"); }
		return retval;

	}
	// $ANTLR end "binaryFunctionName"


	[Conditional("ANTLR_TRACE")]
	protected virtual void EnterRule_number() {}
	[Conditional("ANTLR_TRACE")]
	protected virtual void LeaveRule_number() {}
	// $ANTLR start "number"
	// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:101:1: number : ( DIGIT )+ ;
	[GrammarRule("number")]
	private ParserRuleReturnScope<IToken> number()
	{
		EnterRule_number();
		EnterRule("number", 8);
		TraceIn("number", 8);
	    ParserRuleReturnScope<IToken> retval = new ParserRuleReturnScope<IToken>();
	    retval.Start = (IToken)input.LT(1);

		try { DebugEnterRule(GrammarFileName, "number");
		DebugLocation(101, 11);
		try
		{
			// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:102:2: ( ( DIGIT )+ )
			DebugEnterAlt(1);
			// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:102:4: ( DIGIT )+
			{
			DebugLocation(102, 4);
			// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:102:4: ( DIGIT )+
			int cnt5=0;
			try { DebugEnterSubRule(5);
			while (true)
			{
				int alt5=2;
				try { DebugEnterDecision(5, decisionCanBacktrack[5]);
				int LA5_1 = input.LA(1);

				if ((LA5_1==DIGIT))
				{
					alt5 = 1;
				}


				} finally { DebugExitDecision(5); }
				switch (alt5)
				{
				case 1:
					DebugEnterAlt(1);
					// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:102:5: DIGIT
					{
					DebugLocation(102, 5);
					Match(input,DIGIT,Follow._DIGIT_in_number315); 

					}
					break;

				default:
					if (cnt5 >= 1)
						goto loop5;

					EarlyExitException eee5 = new EarlyExitException( 5, input );
					DebugRecognitionException(eee5);
					throw eee5;
				}
				cnt5++;
			}
			loop5:
				;

			} finally { DebugExitSubRule(5); }


			}

			retval.Stop = (IToken)input.LT(-1);

		}

			catch (RecognitionException e)
			{
				throw;
			}

		finally
		{
			TraceOut("number", 8);
			LeaveRule("number", 8);
			LeaveRule_number();
	    }
	 	DebugLocation(102, 11);
		} finally { DebugExitRule(GrammarFileName, "number"); }
		return retval;

	}
	// $ANTLR end "number"


	[Conditional("ANTLR_TRACE")]
	protected virtual void EnterRule_fileName() {}
	[Conditional("ANTLR_TRACE")]
	protected virtual void LeaveRule_fileName() {}
	// $ANTLR start "fileName"
	// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:104:1: fileName : 'FileName' ;
	[GrammarRule("fileName")]
	private void fileName()
	{
		EnterRule_fileName();
		EnterRule("fileName", 9);
		TraceIn("fileName", 9);
		try { DebugEnterRule(GrammarFileName, "fileName");
		DebugLocation(104, 13);
		try
		{
			// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:105:2: ( 'FileName' )
			DebugEnterAlt(1);
			// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:105:4: 'FileName'
			{
			DebugLocation(105, 4);
			Match(input,12,Follow._12_in_fileName326); 

			}

		}

			catch (RecognitionException e)
			{
				throw;
			}

		finally
		{
			TraceOut("fileName", 9);
			LeaveRule("fileName", 9);
			LeaveRule_fileName();
	    }
	 	DebugLocation(105, 13);
		} finally { DebugExitRule(GrammarFileName, "fileName"); }
		return;

	}
	// $ANTLR end "fileName"


	[Conditional("ANTLR_TRACE")]
	protected virtual void EnterRule_password() {}
	[Conditional("ANTLR_TRACE")]
	protected virtual void LeaveRule_password() {}
	// $ANTLR start "password"
	// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:107:1: password : 'Password' ;
	[GrammarRule("password")]
	private void password()
	{
		EnterRule_password();
		EnterRule("password", 10);
		TraceIn("password", 10);
		try { DebugEnterRule(GrammarFileName, "password");
		DebugLocation(107, 13);
		try
		{
			// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:108:2: ( 'Password' )
			DebugEnterAlt(1);
			// D:\\Patrick\\FigLeaf\\fig-leaf\\FigLeaf.Core\\FigLeafPasswordRule.g:108:4: 'Password'
			{
			DebugLocation(108, 4);
			Match(input,16,Follow._16_in_password335); 

			}

		}

			catch (RecognitionException e)
			{
				throw;
			}

		finally
		{
			TraceOut("password", 10);
			LeaveRule("password", 10);
			LeaveRule_password();
	    }
	 	DebugLocation(108, 13);
		} finally { DebugExitRule(GrammarFileName, "password"); }
		return;

	}
	// $ANTLR end "password"
	#endregion Rules


	#region Follow sets
	private static class Follow
	{
		public static readonly BitSet _password_in_baseArgument67 = new BitSet(new ulong[]{0x0000000000000002UL});
		public static readonly BitSet _fileName_in_baseArgument75 = new BitSet(new ulong[]{0x0000000000000002UL});
		public static readonly BitSet _baseArgument_in_functionArgument99 = new BitSet(new ulong[]{0x0000000000000002UL});
		public static readonly BitSet _number_in_functionArgument112 = new BitSet(new ulong[]{0x0000000000000002UL});
		public static readonly BitSet _function_in_functionArgument124 = new BitSet(new ulong[]{0x0000000000000002UL});
		public static readonly BitSet _unaryFunction_in_function147 = new BitSet(new ulong[]{0x0000000000000002UL});
		public static readonly BitSet _binaryFunction_in_function160 = new BitSet(new ulong[]{0x0000000000000002UL});
		public static readonly BitSet _binaryFunctionName_in_binaryFunction183 = new BitSet(new ulong[]{0x0000000000000080UL});
		public static readonly BitSet _OPENBRACKET_in_binaryFunction185 = new BitSet(new ulong[]{0x00000000001FFE40UL});
		public static readonly BitSet _functionArgument_in_binaryFunction191 = new BitSet(new ulong[]{0x0000000000000020UL});
		public static readonly BitSet _COMMA_in_binaryFunction194 = new BitSet(new ulong[]{0x00000000001FFF40UL});
		public static readonly BitSet _SPACE_in_binaryFunction196 = new BitSet(new ulong[]{0x00000000001FFF40UL});
		public static readonly BitSet _functionArgument_in_binaryFunction203 = new BitSet(new ulong[]{0x0000000000000010UL});
		public static readonly BitSet _CLOSEBRACKET_in_binaryFunction206 = new BitSet(new ulong[]{0x0000000000000002UL});
		public static readonly BitSet _unaryFunctionName_in_unaryFunction228 = new BitSet(new ulong[]{0x0000000000000080UL});
		public static readonly BitSet _OPENBRACKET_in_unaryFunction230 = new BitSet(new ulong[]{0x00000000001FFE40UL});
		public static readonly BitSet _functionArgument_in_unaryFunction236 = new BitSet(new ulong[]{0x0000000000000010UL});
		public static readonly BitSet _CLOSEBRACKET_in_unaryFunction239 = new BitSet(new ulong[]{0x0000000000000002UL});
		public static readonly BitSet _DIGIT_in_number315 = new BitSet(new ulong[]{0x0000000000000042UL});
		public static readonly BitSet _12_in_fileName326 = new BitSet(new ulong[]{0x0000000000000002UL});
		public static readonly BitSet _16_in_password335 = new BitSet(new ulong[]{0x0000000000000002UL});
	}
	#endregion Follow sets
}

} // namespace  FigLeaf.Core.PasswordRules 
