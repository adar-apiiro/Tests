import io.ktor.server.engine.embeddedServer
import io.ktor.server.netty.Netty
import io.ktor.server.routing.routing
import io.ktor.server.application.*
import io.ktor.server.response.respondText
import io.ktor.http.ContentType
import io.ktor.server.routing.get

fun main() {
    embeddedServer(Netty, port = 8080) {
        module()
    }.start(wait = true)
}

fun Application.module() {
    routing {
        get("/") {
            call.respondText("Hello, World!", ContentType.Text.Plain)
        }
    }
}
