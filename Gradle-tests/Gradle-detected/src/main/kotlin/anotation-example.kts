package com.example

import kotlinx.serialization.decodeFromString
import kotlinx.serialization.json.Json
import org.gradle.api.DefaultTask
import org.gradle.api.tasks.TaskAction

open class bla @Inject constructor(
    private val extension: blabla
) : DefaultTask() {

    @TaskAction
    fun writeFunction() {
        println(LOGO())
    }

    private fun LOGO(): String {
        // Implementation of the LOGO function goes here
        return "Your Logo"
    }
}
