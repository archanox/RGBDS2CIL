SECTION "rom0", ROM0
ld sp, $fffe                      ;$0000
xor a                             ;$0003
ld hl, $9fff                      ;$0004
ldd [hl], a                       ;$0007
bit 7, h                          ;$0008
jr nz, $0007                      ;$000a
ld hl, $ff26                      ;$000c
ld c, $11                         ;$000f
ld a, $80                         ;$0011
ldd [hl], a                       ;$0013
ld [$ff00+c], a                   ;$0014
inc c                             ;$0015
ld a, $f3                         ;$0016
ld [$ff00+c], a                   ;$0018
ldd [hl], a                       ;$0019
inc c                             ;$001a
ld a, $c1                         ;$001b
ld [$ff00+c], a                   ;$001d
ld a, $77                         ;$001e
ld [hl], a                        ;$0020
ld a, $fc                         ;$0021
ldh [$ff47], a                    ;$0023
ld a, $91                         ;$0025
ldh [$ff40], a                    ;$0027
ld de, $0043                      ;$0029
call $0073                        ;$002c
cp $34                            ;$002f
jr nz, $0036                      ;$0031
jp $00fc                          ;$0033
ld de, $005b                      ;$0036
call $0073                        ;$0039
cp $34                            ;$003c
jr nz, $003e                      ;$003e
jp $00fc                          ;$0040
call c, $6ecc                     ;$0043
and a, $dd                        ;$0046
db $dd ;<unknown instruction>     ;$0048
reti                              ;$0049
sbc c                             ;$004a
cp e                              ;$004b
cp e                              ;$004c
ld h, a                           ;$004d
ld h, e                           ;$004e
ld l, [hl]                        ;$004f
ld c, $ec                         ;$0050
call z, $dcdd                     ;$0052
sbc c                             ;$0055
sbc a                             ;$0056
cp e                              ;$0057
cp c                              ;$0058
inc sp                            ;$0059
ld a, $00                         ;$005a
nop                               ;$005c
nop                               ;$005d
nop                               ;$005e
halt                              ;$005f
ld h, [hl]                        ;$0060
add a, $31                        ;$0061
nop                               ;$0063
add hl, de                        ;$0064
ld h, [hl]                        ;$0065
rst $38                           ;$0066
ld bc, $3888                      ;$0067
rst $00                           ;$006a
add a, $c8                        ;$006b
nop                               ;$006d
nop                               ;$006e
nop                               ;$006f
nop                               ;$0070
nop                               ;$0071
nop                               ;$0072
ld hl, $011c                      ;$0073
ld a, [de]                        ;$0076
inc de                            ;$0077
cp [hl]                           ;$0078
jr nz, $0082                      ;$0079
inc hl                            ;$007b
ld a, l                           ;$007c
cp $34                            ;$007d
jr nz, $0076                      ;$007f
ret                               ;$0081
ld a, $85                         ;$0082
ret                               ;$0084
rst $38                           ;$0085
rst $38                           ;$0086
rst $38                           ;$0087
rst $38                           ;$0088
rst $38                           ;$0089
rst $38                           ;$008a
rst $38                           ;$008b
rst $38                           ;$008c
rst $38                           ;$008d
rst $38                           ;$008e
rst $38                           ;$008f
rst $38                           ;$0090
rst $38                           ;$0091
rst $38                           ;$0092
rst $38                           ;$0093
rst $38                           ;$0094
rst $38                           ;$0095
rst $38                           ;$0096
rst $38                           ;$0097
rst $38                           ;$0098
rst $38                           ;$0099
rst $38                           ;$009a
rst $38                           ;$009b
rst $38                           ;$009c
rst $38                           ;$009d
rst $38                           ;$009e
rst $38                           ;$009f
rst $38                           ;$00a0
rst $38                           ;$00a1
rst $38                           ;$00a2
rst $38                           ;$00a3
rst $38                           ;$00a4
rst $38                           ;$00a5
rst $38                           ;$00a6
rst $38                           ;$00a7
rst $38                           ;$00a8
rst $38                           ;$00a9
rst $38                           ;$00aa
rst $38                           ;$00ab
rst $38                           ;$00ac
rst $38                           ;$00ad
rst $38                           ;$00ae
rst $38                           ;$00af
rst $38                           ;$00b0
rst $38                           ;$00b1
rst $38                           ;$00b2
rst $38                           ;$00b3
rst $38                           ;$00b4
rst $38                           ;$00b5
rst $38                           ;$00b6
rst $38                           ;$00b7
rst $38                           ;$00b8
rst $38                           ;$00b9
rst $38                           ;$00ba
rst $38                           ;$00bb
rst $38                           ;$00bc
rst $38                           ;$00bd
rst $38                           ;$00be
rst $38                           ;$00bf
rst $38                           ;$00c0
rst $38                           ;$00c1
rst $38                           ;$00c2
rst $38                           ;$00c3
rst $38                           ;$00c4
rst $38                           ;$00c5
rst $38                           ;$00c6
rst $38                           ;$00c7
rst $38                           ;$00c8
rst $38                           ;$00c9
rst $38                           ;$00ca
rst $38                           ;$00cb
rst $38                           ;$00cc
rst $38                           ;$00cd
rst $38                           ;$00ce
rst $38                           ;$00cf
rst $38                           ;$00d0
rst $38                           ;$00d1
rst $38                           ;$00d2
rst $38                           ;$00d3
rst $38                           ;$00d4
rst $38                           ;$00d5
rst $38                           ;$00d6
rst $38                           ;$00d7
rst $38                           ;$00d8
rst $38                           ;$00d9
rst $38                           ;$00da
rst $38                           ;$00db
rst $38                           ;$00dc
rst $38                           ;$00dd
rst $38                           ;$00de
rst $38                           ;$00df
rst $38                           ;$00e0
rst $38                           ;$00e1
rst $38                           ;$00e2
rst $38                           ;$00e3
rst $38                           ;$00e4
rst $38                           ;$00e5
rst $38                           ;$00e6
rst $38                           ;$00e7
rst $38                           ;$00e8
rst $38                           ;$00e9
rst $38                           ;$00ea
rst $38                           ;$00eb
rst $38                           ;$00ec
rst $38                           ;$00ed
rst $38                           ;$00ee
rst $38                           ;$00ef
rst $38                           ;$00f0
rst $38                           ;$00f1
rst $38                           ;$00f2
rst $38                           ;$00f3
rst $38                           ;$00f4
rst $38                           ;$00f5
rst $38                           ;$00f6
rst $38                           ;$00f7
rst $38                           ;$00f8
rst $38                           ;$00f9
rst $38                           ;$00fa
rst $38                           ;$00fb
ld a, $01                         ;$00fc
ldh [$ff50], a                    ;$00fe