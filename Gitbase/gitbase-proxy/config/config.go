package config

import (
	"encoding/json"
	"os"
	"proxy/error_handling"
)

type HostEntry struct {
	DomainKey  string
	DestScheme string
	DestHost   string
	DestPort   string
}

type Configuration struct {
	ProxyHost  string
	CertPath   string
	KeyPath    string
	ProxyTable []HostEntry
}

const configPath = "./settings.json"

var Settings Configuration

func InitConfig() {
	configBytes, err := os.ReadFile(configPath)
	error_handling.HandleCritical(err)
	err = json.Unmarshal(configBytes, &Settings)
	error_handling.HandleCritical(err)
}
