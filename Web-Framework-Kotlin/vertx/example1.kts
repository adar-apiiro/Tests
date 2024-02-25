package com.example

import io.vertx.core.AbstractVerticle
import io.vertx.core.Promise
import io.vertx.ext.web.Router
import io.vertx.ext.web.handler.BodyHandler
import io.vertx.kotlin.coroutines.CoroutineVerticle
import io.vertx.kotlin.coroutines.awaitResult

class MainVerticle : CoroutineVerticle() {

    override suspend fun start() {
        val router = Router.router(vertx).apply {
            route().handler(BodyHandler.create())
            get("/api/hello").handler { ctx ->
                ctx.json(mapOf("message" to "Hello, Vert.x!"))
            }
        }

        val server = vertx.createHttpServer()
            .requestHandler(router)
            .listen(8080)
            .awaitResult()

        println("Server started on port ${server.actualPort()}")
    }
}
