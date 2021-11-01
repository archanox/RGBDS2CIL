SECTION "rom0", ROM0
ld sp, $fffe                      ;$0000
xor a                             ;$0003
ld hl, $9fff                      ;$0004
ldd [hl], a                       ;$0007
bit 7, h                          ;$0008
jr nz, $0007                      ;$000a
ld a, $fc                         ;$000c
ldh [$ff47], a                    ;$000e
ld de, $0104                      ;$0010
ld hl, $8010                      ;$0013
ld a, [de]                        ;$0016
call $0099                        ;$0017
call $009a                        ;$001a
inc de                            ;$001d
ld a, e                           ;$001e
cp $34                            ;$001f
jr nz, $0016                      ;$0021
ld de, $00d8                      ;$0023
ld b, $08                         ;$0026
ld a, [de]                        ;$0028
inc de                            ;$0029
ldi [hl], a                       ;$002a
inc hl                            ;$002b
dec b                             ;$002c
jr nz, $0028                      ;$002d
ld a, $19                         ;$002f
ld [$9910], a                     ;$0031
ld hl, $992f                      ;$0034
ld c, $0c                         ;$0037
dec a                             ;$0039
jr z, $0044                       ;$003a
ldd [hl], a                       ;$003c
dec c                             ;$003d
jr nz, $0039                      ;$003e
ld l, $0f                         ;$0040
jr $0037                          ;$0042
ld a, $91                         ;$0044
ldh [$ff40], a                    ;$0046
ld de, $00b4                      ;$0048
call $00e0                        ;$004b
ld d, $ba                         ;$004e
cp $30                            ;$0050
jr z, $006d                       ;$0052
ld de, $00c6                      ;$0054
call $00e0                        ;$0057
cp $30                            ;$005a
jr nz, $005c                      ;$005c
ld d, $c9                         ;$005e
xor a                             ;$0060
ld [$9910], a                     ;$0061
ld a, $19                         ;$0064
ld [$990d], a                     ;$0066
ld hl, $a000                      ;$0069
ld a, [hl]                        ;$006c
ld hl, $ff26                      ;$006d
ld c, $11                         ;$0070
ld a, $80                         ;$0072
ldd [hl], a                       ;$0074
ld [$ff00+c], a                   ;$0075
inc c                             ;$0076
ld a, $f3                         ;$0077
ld [$ff00+c], a                   ;$0079
ldd [hl], a                       ;$007a
ld a, $77                         ;$007b
ld [hl], a                        ;$007d
inc c                             ;$007e
ld [$ff00+c], a                   ;$007f
ld a, $87                         ;$0080
inc c                             ;$0082
ld [$ff00+c], a                   ;$0083
ld hl, $0a00                      ;$0084
ldh a, [$ff44]                    ;$0087
cp $90                            ;$0089
jr nz, $0087                      ;$008b
dec l                             ;$008d
jr nz, $0087                      ;$008e
dec h                             ;$0090
jr nz, $0087                      ;$0091
ld hl, $0104                      ;$0093
jp $00f1                          ;$0096
ld c, a                           ;$0099
ld b, $04                         ;$009a
push bc                           ;$009c
rl c                              ;$009d
rla                               ;$009f
pop bc                            ;$00a0
rl c                              ;$00a1
rla                               ;$00a3
dec b                             ;$00a4
jr nz, $009c                      ;$00a5
ldi [hl], a                       ;$00a7
inc hl                            ;$00a8
ldi [hl], a                       ;$00a9
inc hl                            ;$00aa
ret                               ;$00ab
nop                               ;$00ac
nop                               ;$00ad
nop                               ;$00ae
nop                               ;$00af
nop                               ;$00b0
nop                               ;$00b1
nop                               ;$00b2
nop                               ;$00b3
ld l, [hl]                        ;$00b4
and a, $dd                        ;$00b5
db $dd ;<unknown instruction>     ;$00b7
reti                              ;$00b8
sbc c                             ;$00b9
cp e                              ;$00ba
cp e                              ;$00bb
ld h, a                           ;$00bc
ld h, e                           ;$00bd
ld l, [hl]                        ;$00be
ld c, $ec                         ;$00bf
call z, $dcdd                     ;$00c1
sbc c                             ;$00c4
sbc a                             ;$00c5
nop                               ;$00c6
nop                               ;$00c7
halt                              ;$00c8
ld h, [hl]                        ;$00c9
add a, $31                        ;$00ca
nop                               ;$00cc
add hl, de                        ;$00cd
ld h, [hl]                        ;$00ce
rst $38                           ;$00cf
ld bc, $3888                      ;$00d0
rst $00                           ;$00d3
add a, $c8                        ;$00d4
nop                               ;$00d6
nop                               ;$00d7
inc a                             ;$00d8
ld b, d                           ;$00d9
cp c                              ;$00da
and l                             ;$00db
cp c                              ;$00dc
and l                             ;$00dd
ld b, d                           ;$00de
inc a                             ;$00df
ld hl, $011e                      ;$00e0
ld a, [de]                        ;$00e3
inc de                            ;$00e4
cp [hl]                           ;$00e5
jr nz, $00ef                      ;$00e6
inc hl                            ;$00e8
ld a, l                           ;$00e9
cp $30                            ;$00ea
jr nz, $00e3                      ;$00ec
ret                               ;$00ee
xor a                             ;$00ef
ret                               ;$00f0
ld b, $2f                         ;$00f1
ld a, d                           ;$00f3
add [hl]                          ;$00f4
inc hl                            ;$00f5
dec b                             ;$00f6
jr nz, $00f4                      ;$00f7
add [hl]                          ;$00f9
jr nz, $00fa                      ;$00fa
ld a, $01                         ;$00fc
ldh [$ff50], a                    ;$00fe