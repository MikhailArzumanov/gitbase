package response

import (
	"fmt"
	"io"
	"net/http"
)

func GetBaseData(response *http.Response) (string, int) {
	respBytes, err := io.ReadAll(response.Body)
	if err != nil {
		fmt.Println(err)
	}
	var respBody = string(respBytes)
	var respCode = response.StatusCode

	return respBody, respCode
}

func WriteRespHeaders(
	resp *http.Response,
	writer http.ResponseWriter,
) {
	for name, values := range resp.Header {
		for _, value := range values {
			writer.Header().Set(name, value)
		}
	}
}
func WriteRespCode(
	respCode int,
	method string,
	writer http.ResponseWriter,
) {
	if respCode != 200 && method != "OPTIONS" {
		writer.WriteHeader(respCode)
	}
}
func WriteRespBody(
	writer http.ResponseWriter,
	respBody string,
) {
	fmt.Fprint(writer, respBody)
}

func SendResponseToClient(
	resp *http.Response,
	method string,
	writer http.ResponseWriter,
) {
	respBody, respCode := GetBaseData(resp)
	WriteRespHeaders(resp, writer)
	WriteRespCode(respCode, method, writer)
	WriteRespBody(writer, respBody)
}
