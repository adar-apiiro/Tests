import io.javalin.Javalin
import io.javalin.http.Context

fun main() {
    val app = Javalin.create().start(7000)
    app.get("/") { ctx: Context -> ctx.result("Hello, Javalin!") }
}
