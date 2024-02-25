package com.example.routes
package com.example.plugins

import com.example.routes.*
import io.ktor.server.application.*
import io.ktor.server.routing.*
import io.ktor.server.routing.*
import io.ktor.server.request.*
import io.ktor.server.response.*
import io.ktor.server.routing.*
import io.ktor.http.*
import com.example.models.*

fun Route.customerRouting() {
    route("/customer") {
        get {

        }
        get("{id?}") {

        }
        post {

        }
        delete("{id?}") {

        }
    }
}

fun Route.customerRouting() {
    route("/customer") {
        get {
            if (customerStorage.isNotEmpty()) {
                call.respond(customerStorage)
            } else {
                call.respondText("No customers found", status = HttpStatusCode.OK)
            }
        }
    }
}

fun Route.listOrdersRoute() {
    get("/order") {
        if (orderStorage.isNotEmpty()) {
            call.respond(orderStorage)
        }
    }

fun Route.getOrderRoute() {
    get("/order/{id?}") {
        val id = call.parameters["id"] ?: return@get call.respondText("Bad Request", status = HttpStatusCode.BadRequest)
        val order = orderStorage.find { it.number == id } ?: return@get call.respondText(
            "Not Found",
            status = HttpStatusCode.NotFound
        )
        call.respond(order)
    }
}



fun Application.configureRouting() {
    routing {
        customerRouting()
        listOrdersRoute()
        getOrderRoute()
        totalizeOrderRoute()
    }
}
