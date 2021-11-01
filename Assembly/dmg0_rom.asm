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
ld a, $77                         ;$001a
ld [hl], a                        ;$001c
ld a, $fc                         ;$001d
ldh [$ff47], a                    ;$001f
ld hl, $0104                      ;$0021
push hl                           ;$0024
ld de, $00cb                      ;$0025
ld a, [de]                        ;$0028
inc de                            ;$0029
cp [hl]                           ;$002a
jr nz, $0098                      ;$002b
inc hl                            ;$002d
ld a, l                           ;$002e
cp $34                            ;$002f
jr nz, $0028                      ;$0031
ld b, $19                         ;$0033
ld a, b                           ;$0035
add [hl]                          ;$0036
inc hl                            ;$0037
dec b                             ;$0038
jr nz, $0036                      ;$0039
add [hl]                          ;$003b
jr nz, $0098                      ;$003c
pop de                            ;$003e
ld hl, $8010                      ;$003f
ld a, [de]                        ;$0042
call $00a9                        ;$0043
call $00aa                        ;$0046
inc de                            ;$0049
ld a, e                           ;$004a
cp $34                            ;$004b
jr nz, $0042                      ;$004d
ld a, $18                         ;$004f
ld hl, $992f                      ;$0051
ld c, $0c                         ;$0054
ldd [hl], a                       ;$0056
dec a                             ;$0057
jr z, $0063                       ;$0058
dec c                             ;$005a
jr nz, $0056                      ;$005b
ld de, $ffec                      ;$005d
add hl, de                        ;$0060
jr $0054                          ;$0061
ld h, a                           ;$0063
ld a, $64                         ;$0064
ld d, a                           ;$0066
ldh [$ff42], a                    ;$0067
ld a, $91                         ;$0069
ldh [$ff40], a                    ;$006b
inc b                             ;$006d
ld e, $02                         ;$006e
call $00bc                        ;$0070
ld c, $13                         ;$0073
inc h                             ;$0075
ld a, h                           ;$0076
ld e, $83                         ;$0077
cp $62                            ;$0079
jr z, $0083                       ;$007b
ld e, $c1                         ;$007d
cp $64                            ;$007f
jr nz, $0089                      ;$0081
ld a, e                           ;$0083
ld [$ff00+c], a                   ;$0084
inc c                             ;$0085
ld a, $87                         ;$0086
ld [$ff00+c], a                   ;$0088
ldh a, [$ff42]                    ;$0089
sub b                             ;$008b
ldh [$ff42], a                    ;$008c
dec d                             ;$008e
jr nz, $006e                      ;$008f
dec b                             ;$0091
jr nz, $00fd                      ;$0092
ld d, $20                         ;$0094
jr $006e                          ;$0096
ld a, $91                         ;$0098
ldh [$ff40], a                    ;$009a
ld e, $14                         ;$009c
call $00bc                        ;$009e
ldh a, [$ff47]                    ;$00a1
xor $ff                           ;$00a3
ldh [$ff47], a                    ;$00a5
jr $009c                          ;$00a7
ld c, a                           ;$00a9
ld b, $04                         ;$00aa
push bc                           ;$00ac
rl c                              ;$00ad
rla                               ;$00af
pop bc                            ;$00b0
rl c                              ;$00b1
rla                               ;$00b3
dec b                             ;$00b4
jr nz, $00ac                      ;$00b5
ldi [hl], a                       ;$00b7
inc hl                            ;$00b8
ldi [hl], a                       ;$00b9
inc hl                            ;$00ba
ret                               ;$00bb
ld c, $0c                         ;$00bc
ldh a, [$ff44]                    ;$00be
cp $90                            ;$00c0
jr nz, $00be                      ;$00c2
dec c                             ;$00c4
jr nz, $00be                      ;$00c5
dec e                             ;$00c7
jr nz, $00bc                      ;$00c8
ret                               ;$00ca
adc a, $ed                        ;$00cb
ld h, [hl]                        ;$00cd
ld h, [hl]                        ;$00ce
call z, $000d                     ;$00cf
dec bc                            ;$00d2
inc bc                            ;$00d3
ld [hl], e                        ;$00d4
nop                               ;$00d5
add e                             ;$00d6
nop                               ;$00d7
inc c                             ;$00d8
nop                               ;$00d9
dec c                             ;$00da
nop                               ;$00db
ld [$1f11], sp                    ;$00dc
adc b                             ;$00df
adc c                             ;$00e0
nop                               ;$00e1
ld c, $dc                         ;$00e2
call z, $e66e                     ;$00e4
db $dd ;<unknown instruction>     ;$00e7
db $dd ;<unknown instruction>     ;$00e8
reti                              ;$00e9
sbc c                             ;$00ea
cp e                              ;$00eb
cp e                              ;$00ec
ld h, a                           ;$00ed
ld h, e                           ;$00ee
ld l, [hl]                        ;$00ef
ld c, $ec                         ;$00f0
call z, $dcdd                     ;$00f2
sbc c                             ;$00f5
sbc a                             ;$00f6
cp e                              ;$00f7
cp c                              ;$00f8
inc sp                            ;$00f9
ld a, $ff                         ;$00fa
rst $38                           ;$00fc
inc a                             ;$00fd
ldh [$ff50], a                    ;$00fe