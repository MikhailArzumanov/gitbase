package proxy

import (
	"net/http"
	"proxy/error_handling"
	"proxy/response"
	"proxy/target"
)

func getServerResp(req *http.Request) *http.Response {
	var client = &http.Client{}
	var resp *http.Response
	resp, err := client.Do(req)
	error_handling.Handle(err)
	return resp
}

func ProxyHandler(
	address string,
	respWriter http.ResponseWriter,
	sourceRequest *http.Request,
) {
	targetRequest := target.FormTargetRequest(sourceRequest, address)
	resp := getServerResp(targetRequest)
	response.SendResponseToClient(resp, sourceRequest.Method, respWriter)
}
