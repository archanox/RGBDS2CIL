SECTION "UndefinedEntitiesTest", ROM0

TestUndefinedLabelCall:
    JP NonExistentLabel

TestUndefinedConstantUsage:
    LD A, NonExistentConstant

TestUndefinedMacroCall:
    NonExistentMacro "arg1", "arg2"
