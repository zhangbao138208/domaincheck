package controller

// 数据库
type DB_table_users struct{
	UserName string `json:"username"`
	Password string `json:"password"`
	Product string `json:"product"`
	IsManager string `json:"is_manager"`
}

type DB_table_domains struct{
	DomainIndex int `json:"domain_index"`
	Domain string `json:"domain"`
	IsNeedCheck int `json:"is_need_check"`
	CheckpointIndex int `json:"checkpoint_index"`
	Product string `json:"product"`
	Creator string `json:"creator"`
	UpdateDate string `json:"update_date"`
	Comment string `json:"comment"`
}

type DB_table_checkpoints struct{
	CheckpointIndex int `json:"checkpoint_index"`
	Product string `json:"product"`
	Creator string `json:"creator"`
	CheckPath string `json:"check_path"`
	CheckString string `json:"check_string"`
}

type DB_Table_logs struct{
	LogIndex int `json:"log_index"`
	Domain string `json:"domain"`
	CheckIP string `json:"check_ip"`
	Creator string `json:"creator"`
	CheckDate string `json:"check_date"`
	Result string `json:"result"`
	ClientID string `json:"client_id"`
	Product string `json:"product"`
	PrintScreen [] byte `json:"printscreen"`
}

type DB_Table_IPList struct{
	Index int `json:"index"`
	IP string `json:"ip"`
	IsManager int `json:"is_manager"`
	Comment string `json:"comment"`
	IsUseful int `json:"is_useful"`
	CustomIndex int `json:"custom_index"`
	Product string `json:"product"`
}

/*
type tmp_DB_Table_logs struct{
	LogIndex int `json:"log_index"`
	Domain string `json:"domain"`
	CheckIP string `json:"check_ip"`
	Creator string `json:"creator"`
	CheckDate string `json:"check_date"`
	Result string `json:"result"`
	PrintScreen string `json:"printscreen"`
}
*/