import org.http4k.core.HttpHandler
import org.http4k.core.Method.GET
import org.http4k.core.Request
import org.http4k.core.Response
import org.http4k.core.Status.Companion.OK
import org.http4k.routing.bind
import org.http4k.routing.routes
import org.http4k.server.Jetty
import org.http4k.server.asServer

class Http4kServer {
    fun startServer() {
        val app: HttpHandler = routes(
            "/" bind GET to { _: Request -> Response(OK).body("Hello, Http4k!") }
        )

        val server = app.asServer(Jetty(8080)).start()

        println("Server started on " + server.port())
    }
}

fun main() {
    val server = Http4kServer()
    server.startServer()
    val server1 = routes()
}
