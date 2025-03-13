package target

import (
	"bytes"
	"io"
	"net/http"
	"proxy/error_handling"
)

func setRequestHeaders(sourceRequest, targetRequest *http.Request) {
	for name, values := range sourceRequest.Header {
		for _, value := range values {
			targetRequest.Header.Set(name, value)
		}
	}
}

func readBody(src *http.Request) []byte {
	body, err := io.ReadAll(src.Body)
	error_handling.Handle(err)
	return body
}

func getUrl(
	sourceRequest *http.Request,
	destHostAddress string,
) string {
	srcUrl := sourceRequest.URL.String()
	url := destHostAddress + srcUrl
	return url
}

func getBodyBfr(sourceRequest *http.Request) io.Reader {
	body := readBody(sourceRequest)
	bodyBfr := bytes.NewBuffer([]byte(body))
	return bodyBfr
}

func createRequest(
	sourceRequest *http.Request,
	destHostAddress string,
) *http.Request {

	method, url, body :=
		sourceRequest.Method,
		getUrl(sourceRequest, destHostAddress),
		getBodyBfr(sourceRequest)

	theRequest, err :=
		http.NewRequest(method, url, body)
	error_handling.Handle(err)

	return theRequest
}

func FormTargetRequest(
	sourceRequest *http.Request,
	destHostAddress string,
) *http.Request {
	destRequest := createRequest(sourceRequest, destHostAddress)
	setRequestHeaders(sourceRequest, destRequest)
	return destRequest
}
