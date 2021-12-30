# RGBDS2CIL

![.NET](https://github.com/archanox/RGBDS2CIL/workflows/.NET/badge.svg)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/8f09df7eff4a4c2fa6398596c7621347)](https://app.codacy.com/gh/archanox/RGBDS2CIL?utm_source=github.com&utm_medium=referral&utm_content=archanox/RGBDS2CIL&utm_campaign=Badge_Grade_Settings)
[![Codacy Badge](https://app.codacy.com/project/badge/Coverage/0727a59999f846388a16ee2a21652327)](https://www.codacy.com/gh/archanox/RGBDS2CIL/dashboard?utm_source=github.com&utm_medium=referral&utm_content=archanox/RGBDS2CIL&utm_campaign=Badge_Coverage)
[![codecov](https://codecov.io/gh/archanox/RGBDS2CIL/branch/master/graph/badge.svg?token=3A2O2AWWMD)](https://codecov.io/gh/archanox/RGBDS2CIL)
[![Build status](https://ci.appveyor.com/api/projects/status/jw87a3xpvde6h65h?svg=true)](https://ci.appveyor.com/project/archanox/rgbds2cil)
[![Join the chat at https://gitter.im/RGBDS2CIL/community](https://badges.gitter.im/RGBDS2CIL/community.svg)](https://gitter.im/RGBDS2CIL/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
[![Codeac](https://static.codeac.io/badges/2-329877684.svg "Codeac")](https://app.codeac.io/github/archanox/RGBDS2CIL)
[![Maintainability](https://api.codeclimate.com/v1/badges/7e95c773297fee1d0fc5/maintainability)](https://codeclimate.com/github/archanox/RGBDS2CIL/maintainability)
[![Total alerts](https://img.shields.io/lgtm/alerts/g/archanox/RGBDS2CIL.svg?logo=lgtm&logoWidth=18)](https://lgtm.com/projects/g/archanox/RGBDS2CIL/alerts/)

Conversion of RGBDS ASM to CIL/C#

Sample ASM Input:
```assembly
assert_valid_rgb: MACRO
    rept _NARG
        assert 0 <= (\1) && (\1) <= 31, "RGB channel must be 0-31"
        shift
    endr
ENDM
    
RGB: MACRO
    rept _NARG / 3
        assert_valid_rgb \1, \2, \3
        dw palred (\1) + palgreen (\2) + palblue (\3)
        shift 3
    endr
ENDM
    
palred   EQUS "(1 << 0) *"
palgreen EQUS "(1 << 5) *"
palblue  EQUS "(1 << 10) *" 
    
;Graphics data header macro
;Format:
;1:bank number
;2:bank data offset
;3:start vram address
;4:type(0: uncompressed, 1: compressed w/ header, 2: compressed no header)
;5:length (not needed if type is 1)
gfxheader: MACRO
    db \1
    dw \2
    dw \3
    IF \4 == 1
        db 0
    ELSE 
        db (\5 & $FF)
    ENDC
    IF \4 == 1
        db $80
    ELIF \4 == 2
        db $80 | (\5 >> 8)
    ELSE
        db (\5 >> 8)
    ENDC
ENDM
```

Sample [WIP] C# Output:
```csharp
namespace macros
{
	public class Gfx
	{
		void Assert_Valid_Rgb(params object[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{
				Debug.Assert(0 <= (args[0]) && (args[0]) <= 31, "RGB channel must be 0-31");
				Shift();
			}
		}

		void RGB(params object[] args)
		{
			for (int i = 0; i < args.Length / 3; i++)
			{
				/* assert_valid_rgb args[0], args[1], args[2] */
				Define(typeof(System.Int16), palred (args[0]) + palgreen (args[1]) + palblue (args[2]));
				Shift();
			}
		}

		const string palred = "(1 << 0) *";
		const string palgreen = "(1 << 5) *";
		const string palblue = "(1 << 10) *";

		// Graphics data header macro
		// Format:
		// 1:bank number
		// 2:bank data offset
		// 3:start vram address
		// 4:type(0: uncompressed, 1: compressed w/ header, 2: compressed no header)
		// 5:length (not needed if type is 1)
		void Gfxheader(params object[] args)
		{
			Define(typeof(System.Byte), args[0]);
			Define(typeof(System.Int16), args[1]);
			Define(typeof(System.Int16), args[2]);
			if (args[3] == 1)
			{
				Define(typeof(System.Byte), 0);
			}
			else
			{
				Define(typeof(System.Byte), (args[4] & 0xFF));
			}
			if (args[3] == 1)
			{
				Define(typeof(System.Byte), 0x80);
			}
			else if (args[3] == 2)
			{
				Define(typeof(System.Byte), 0x80 | (args[4] >> 8));
			}
			else
			{
				Define(typeof(System.Byte), (args[4] >> 8));
			}
		}
	}
}
```