SECTION "rom0", ROM0
ld sp, $fffe                      ;$0000
ld hl, $8000                      ;$0003
ldi [hl], a                       ;$0006
bit 5, h                          ;$0007
jr z, $0006                       ;$0009
ld a, $80                         ;$000b
ldh [$ff26], a                    ;$000d
ldh [$ff11], a                    ;$000f
ld a, $f3                         ;$0011
ldh [$ff12], a                    ;$0013
ldh [$ff25], a                    ;$0015
ld a, $77                         ;$0017
ldh [$ff24], a                    ;$0019
ld a, $54                         ;$001b
ldh [$ff47], a                    ;$001d
ld de, $0104                      ;$001f
ld hl, $8010                      ;$0022
ld a, [de]                        ;$0025
ld b, a                           ;$0026
call $00a2                        ;$0027
call $00a2                        ;$002a
inc de                            ;$002d
ld a, e                           ;$002e
xor $34                           ;$002f
jr nz, $0025                      ;$0031
ld de, $00d1                      ;$0033
ld c, $08                         ;$0036
ld a, [de]                        ;$0038
inc de                            ;$0039
ldi [hl], a                       ;$003a
inc hl                            ;$003b
dec c                             ;$003c
jr nz, $0038                      ;$003d
ld a, $19                         ;$003f
ld [$9910], a                     ;$0041
ld hl, $992f                      ;$0044
ld c, $0c                         ;$0047
dec a                             ;$0049
jr z, $0054                       ;$004a
ldd [hl], a                       ;$004c
dec c                             ;$004d
jr nz, $0049                      ;$004e
ld l, $0f                         ;$0050
jr $0049                          ;$0052
ld a, $1e                         ;$0054
ldh [$ff42], a                    ;$0056
ld a, $91                         ;$0058
ldh [$ff40], a                    ;$005a
ld d, $89                         ;$005c
ld c, $0f                         ;$005e
call $00b7                        ;$0060
ld a, d                           ;$0063
sra a                             ;$0064
sra a                             ;$0066
ldh [$ff42], a                    ;$0068
ld a, d                           ;$006a
add c                             ;$006b
ld d, a                           ;$006c
ld a, c                           ;$006d
cp $08                            ;$006e
jr nz, $0076                      ;$0070
ld a, $a8                         ;$0072
ldh [$ff47], a                    ;$0074
dec c                             ;$0076
jr nz, $0060                      ;$0077
ld a, $fc                         ;$0079
ldh [$ff47], a                    ;$007b
ld a, $83                         ;$007d
call $00ca                        ;$007f
ld b, $05                         ;$0082
call $00c3                        ;$0084
ld a, $c1                         ;$0087
call $00ca                        ;$0089
ld b, $3c                         ;$008c
call $00c3                        ;$008e
ld hl, $01b0                      ;$0091
push hl                           ;$0094
pop af                            ;$0095
ld hl, $014d                      ;$0096
ld bc, $0013                      ;$0099
ld de, $00d8                      ;$009c
jp $00fe                          ;$009f
ld a, $04                         ;$00a2
ld c, $00                         ;$00a4
sla b                             ;$00a6
push af                           ;$00a8
rl c                              ;$00a9
pop af                            ;$00ab
rl c                              ;$00ac
dec a                             ;$00ae
jr nz, $00a6                      ;$00af
ld a, c                           ;$00b1
ldi [hl], a                       ;$00b2
inc hl                            ;$00b3
ldi [hl], a                       ;$00b4
inc hl                            ;$00b5
ret                               ;$00b6
push hl                           ;$00b7
ld hl, $ff0f                      ;$00b8
res 0, [hl]                       ;$00bb
bit 0, [hl]                       ;$00bd
jr z, $00bd                       ;$00bf
pop hl                            ;$00c1
ret                               ;$00c2
call $00b7                        ;$00c3
dec b                             ;$00c6
jr nz, $00c3                      ;$00c7
ret                               ;$00c9
ldh [$ff13], a                    ;$00ca
ld a, $87                         ;$00cc
ldh [$ff14], a                    ;$00ce
ret                               ;$00d0
inc a                             ;$00d1
ld b, d                           ;$00d2
cp c                              ;$00d3
and l                             ;$00d4
cp c                              ;$00d5
and l                             ;$00d6
ld b, d                           ;$00d7
inc a                             ;$00d8
nop                               ;$00d9
nop                               ;$00da
nop                               ;$00db
nop                               ;$00dc
nop                               ;$00dd
nop                               ;$00de
nop                               ;$00df
nop                               ;$00e0
nop                               ;$00e1
nop                               ;$00e2
nop                               ;$00e3
nop                               ;$00e4
nop                               ;$00e5
nop                               ;$00e6
nop                               ;$00e7
nop                               ;$00e8
nop                               ;$00e9
nop                               ;$00ea
nop                               ;$00eb
nop                               ;$00ec
nop                               ;$00ed
nop                               ;$00ee
nop                               ;$00ef
nop                               ;$00f0
nop                               ;$00f1
nop                               ;$00f2
nop                               ;$00f3
nop                               ;$00f4
nop                               ;$00f5
nop                               ;$00f6
nop                               ;$00f7
nop                               ;$00f8
nop                               ;$00f9
nop                               ;$00fa
nop                               ;$00fb
nop                               ;$00fc
nop                               ;$00fd
ldh [$ff50], a                    ;$00fe