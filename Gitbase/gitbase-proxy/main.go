package main

import (
	"fmt"
	"net/http"
	"proxy/config"
	"proxy/router"
)

func listen() {
	http.HandleFunc("/", router.RouteRequest)
	err := http.ListenAndServeTLS(
		config.Settings.ProxyHost,
		config.Settings.CertPath,
		config.Settings.KeyPath,
		nil,
	)
	fmt.Println(err)
}

func main() {
	config.InitConfig()
	fmt.Println("== Config has been loaded ==")
	fmt.Println("== Listening is starting ==")
	listen()
}
