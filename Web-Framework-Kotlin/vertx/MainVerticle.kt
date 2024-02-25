package com.example

import io.vertx.ext.web.Router
import io.vertx.ext.web.handler.BodyHandler
import io.vertx.kotlin.coroutines.CoroutineVerticle

class MainVerticle : CoroutineVerticle() {

    override suspend fun start() {
        val router = Router.router(vertx).apply {
            route().handler(BodyHandler.create())
            get("/api/hello").handler { ctx ->
                ctx.json(mapOf("message" to "Hello from Vert.x and Kotlin!"))
            }
        }

        vertx.createHttpServer()
            .requestHandler(router)
            .listen(8080) { result ->
                if (result.succeeded()) {
                    println("Server listening on port 8080")
                } else {
                    println("Failed to start server: ${result.cause()}")
                }
            }
    }
}
