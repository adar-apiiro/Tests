class ExampleClass {
    fun publicMethod() {
        internalMethod() // Internal method call within the same class
    }
    
    private fun internalMethod() {
        println("Internal method called")
    }
}

fun main() {
    val instance = ExampleClass()
    instance.publicMethod() // This will internally call internalMethod()
}
