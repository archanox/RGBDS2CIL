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
ld de, $0104                      ;$0021
ld hl, $8010                      ;$0024
ld a, [de]                        ;$0027
call $0095                        ;$0028
call $0096                        ;$002b
inc de                            ;$002e
ld a, e                           ;$002f
cp $34                            ;$0030
jr nz, $0027                      ;$0032
ld de, $00d8                      ;$0034
ld b, $08                         ;$0037
ld a, [de]                        ;$0039
inc de                            ;$003a
ldi [hl], a                       ;$003b
inc hl                            ;$003c
dec b                             ;$003d
jr nz, $0039                      ;$003e
ld a, $19                         ;$0040
ld [$9910], a                     ;$0042
ld hl, $992f                      ;$0045
ld c, $0c                         ;$0048
dec a                             ;$004a
jr z, $0055                       ;$004b
ldd [hl], a                       ;$004d
dec c                             ;$004e
jr nz, $004a                      ;$004f
ld l, $0f                         ;$0051
jr $0048                          ;$0053
ld h, a                           ;$0055
ld a, $64                         ;$0056
ld d, a                           ;$0058
ldh [$ff42], a                    ;$0059
ld a, $91                         ;$005b
ldh [$ff40], a                    ;$005d
inc b                             ;$005f
ld e, $02                         ;$0060
ld c, $0c                         ;$0062
ldh a, [$ff44]                    ;$0064
cp $90                            ;$0066
jr nz, $0064                      ;$0068
dec c                             ;$006a
jr nz, $0064                      ;$006b
dec e                             ;$006d
jr nz, $0062                      ;$006e
ld c, $13                         ;$0070
inc h                             ;$0072
ld a, h                           ;$0073
ld e, $83                         ;$0074
cp $62                            ;$0076
jr z, $0080                       ;$0078
ld e, $c1                         ;$007a
cp $64                            ;$007c
jr nz, $0086                      ;$007e
ld a, e                           ;$0080
ld [$ff00+c], a                   ;$0081
inc c                             ;$0082
ld a, $87                         ;$0083
ld [$ff00+c], a                   ;$0085
ldh a, [$ff42]                    ;$0086
sub b                             ;$0088
ldh [$ff42], a                    ;$0089
dec d                             ;$008b
jr nz, $0060                      ;$008c
dec b                             ;$008e
jr nz, $00e0                      ;$008f
ld d, $20                         ;$0091
jr $0060                          ;$0093
ld c, a                           ;$0095
ld b, $04                         ;$0096
push bc                           ;$0098
rl c                              ;$0099
rla                               ;$009b
pop bc                            ;$009c
rl c                              ;$009d
rla                               ;$009f
dec b                             ;$00a0
jr nz, $0098                      ;$00a1
ldi [hl], a                       ;$00a3
inc hl                            ;$00a4
ldi [hl], a                       ;$00a5
inc hl                            ;$00a6
ret                               ;$00a7
adc a, $ed                        ;$00a8
ld h, [hl]                        ;$00aa
ld h, [hl]                        ;$00ab
call z, $000d                     ;$00ac
dec bc                            ;$00af
inc bc                            ;$00b0
ld [hl], e                        ;$00b1
nop                               ;$00b2
add e                             ;$00b3
nop                               ;$00b4
inc c                             ;$00b5
nop                               ;$00b6
dec c                             ;$00b7
nop                               ;$00b8
ld [$1f11], sp                    ;$00b9
adc b                             ;$00bc
adc c                             ;$00bd
nop                               ;$00be
ld c, $dc                         ;$00bf
call z, $e66e                     ;$00c1
db $dd ;<unknown instruction>     ;$00c4
db $dd ;<unknown instruction>     ;$00c5
reti                              ;$00c6
sbc c                             ;$00c7
cp e                              ;$00c8
cp e                              ;$00c9
ld h, a                           ;$00ca
ld h, e                           ;$00cb
ld l, [hl]                        ;$00cc
ld c, $ec                         ;$00cd
call z, $dcdd                     ;$00cf
sbc c                             ;$00d2
sbc a                             ;$00d3
cp e                              ;$00d4
cp c                              ;$00d5
inc sp                            ;$00d6
ld a, $3c                         ;$00d7
ld b, d                           ;$00d9
cp c                              ;$00da
and l                             ;$00db
cp c                              ;$00dc
and l                             ;$00dd
ld b, d                           ;$00de
inc a                             ;$00df
ld hl, $0104                      ;$00e0
ld de, $00a8                      ;$00e3
ld a, [de]                        ;$00e6
inc de                            ;$00e7
cp [hl]                           ;$00e8
jr nz, $00e9                      ;$00e9
inc hl                            ;$00eb
ld a, l                           ;$00ec
cp $34                            ;$00ed
jr nz, $00e6                      ;$00ef
ld b, $19                         ;$00f1
ld a, b                           ;$00f3
add [hl]                          ;$00f4
inc hl                            ;$00f5
dec b                             ;$00f6
jr nz, $00f4                      ;$00f7
add [hl]                          ;$00f9
jr nz, $00fa                      ;$00fa
ld a, $01                         ;$00fc
ldh [$ff50], a                    ;$00fe