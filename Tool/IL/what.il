.method public static void What() cil managed
{
    .entrypoint
    .maxstack 1
    ldstr "What"
    call void [mscorlib]System.Console::WriteLine(string)

    ldc.i4 123
    ret
}