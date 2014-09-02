grammar FigLeafPasswordRule;

options
{
	language=CSharp2;
}

@parser::namespace { FigLeaf.Core.PasswordRules }

@lexer::namespace { FigLeaf.Core.PasswordRules }

@header
{
	using FigLeaf.Core;
	using FigLeaf.Core.PasswordRules;
}

@lexer::members 
{
	public override void EmitErrorMessage(string s)
	{
		throw new RecognitionException(s, input);
	}
}

@rulecatch
{
	catch (RecognitionException e)
	{
		throw;
	}
}
	
baseArgument[string parseFileName, string parsePassword] returns[string value]
	: password
	{
		$value = parsePassword;
	}
	| fileName
	{
		$value = parseFileName;
	}
	;

public functionArgument[string parseFileName, string parsePassword] returns[string value]
	: ba = baseArgument[parseFileName, parsePassword]
	{
		$value = $ba.value;
	}
	| n = number
	{
		$value = $n.text;
	}
	| f = function[parseFileName, parsePassword]
	{
		$value = $f.value;
	}
	;

function[string parseFileName, string parsePassword] returns[string value]
	: uf = unaryFunction[parseFileName, parsePassword]
	{
		$value = $uf.value;
	}
	| bf = binaryFunction[parseFileName, parsePassword]
	{
		$value = $bf.value;
	}
	;

binaryFunction[string parseFileName, string parsePassword] returns[string value]
	: func = binaryFunctionName OPENBRACKET arg1 = functionArgument[parseFileName, parsePassword] COMMA SPACE* arg2 = functionArgument[parseFileName, parsePassword] CLOSEBRACKET
	{
		System.Func<string, string, string> function = Functions.GetBinaryFunction($func.text);
		$value = function($arg1.value, $arg2.value);
	}
	;

unaryFunction[string parseFileName, string parsePassword] returns[string value]
	: func = unaryFunctionName OPENBRACKET arg = functionArgument[parseFileName, parsePassword] CLOSEBRACKET
	{
		System.Func<string, string> function = Functions.GetUnaryFunction($func.text);
		$value = function($arg.value);
	}
	;
	
unaryFunctionName
	: 'Reverse'
	| 'Upper'
	| 'Lower'
	| 'Digits'
	| 'RemoveFileExtension'
	| 'FileExtension'
	| 'Len';
	
binaryFunctionName
	: 'Add'
	| 'Left'
	| 'Right';

number 
	: (DIGIT)+;

fileName
	: 'FileName';

password
	: 'Password';

DIGIT 	
	: '0'..'9';

SPACE
	: ' ';

COMMA
	: ',';

OPENBRACKET
	: '(';

CLOSEBRACKET	
	: ')';


