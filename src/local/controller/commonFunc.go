package controller

import(
	"net"
	"net/http"
	"github.com/go-ozzo/ozzo-dbx"
	"pkg/dbcontext"
	"bytes"
    "crypto/aes"
    "crypto/cipher"
    "fmt"
    "strconv"
)

func GetIP(req *http.Request) string {
	remoteAddr := req.RemoteAddr
	if ip := req.Header.Get("X-Real-Ip"); ip != ""{
		remoteAddr = ip
	} else if ip := req.Header.Get("X-Forwarded-For"); ip != ""{
		remoteAddr = ip
	} else{
		remoteAddr, _, _ = net.SplitHostPort(remoteAddr)
	}

	if remoteAddr == "::1"{
		remoteAddr = "127.0.0.1"
	}
	return remoteAddr
}

func IsAllowedServerIP(req *http.Request, db *dbcontext.DB) bool {
	clientIP := GetIP(req)
	q := db.DB().Select("index", "ip", "is_manager", "comment", "is_useful").
			From("tb_iplist").
			Where(dbx.HashExp{"ip": clientIP, "is_manager": 1})
	var ipLists [] DB_Table_IPList
	err := q.All(&ipLists)
	if err != nil {
		return false
	}
	// 无数据
	var ipListNum = len(ipLists)
	if ipListNum <= 0 {
		return false
	}
	// 有数据，但仍然无效
	if ipLists[0].IsUseful == 0 {
		return false
	}
	return true
}

func IsAllowedClientMAC(MAC string, db *dbcontext.DB) bool {
	q := db.DB().Select("index", "ip", "is_manager", "comment", "is_useful").
			From("tb_iplist").
			Where(dbx.HashExp{"ip": MAC, "is_manager": 0})
	var ipLists [] DB_Table_IPList
	err := q.All(&ipLists)
	if err != nil {
		return false
	}
	// 无数据
	var ipListNum = len(ipLists)
	if ipListNum <= 0 {
		return false
	}
	// 有数据，但仍然无效
	if ipLists[0].IsUseful == 0 {
		return false
	}
	return true
}

func SimpleAesEncrypt(value string) (string, error) {
	return aesEncrypt(value, "freeknight", "1234567890")
}

func SimpleAesDecrypt(value string) (string, error) {
	return aesDecrypt(value, "freeknight", "1234567890")
}
//AES加密
//iv为空则采用ECB模式，否则采用CBC模式
func aesEncrypt(value, secretKey, iv string) (string, error) {
    if value == "" {
        return "", nil
    }

    //根据秘钥生成16位的秘钥切片
    keyBytes := make([]byte, aes.BlockSize)
    copy(keyBytes, []byte(secretKey))
    //获取block
    block, err := aes.NewCipher(keyBytes)
    if err != nil {
        return "", err
    }

    blocksize := block.BlockSize()
    valueBytes := []byte(value)

    //填充
    fillsize := blocksize - len(valueBytes)%blocksize
    repeat := bytes.Repeat([]byte{byte(fillsize)}, fillsize)
    valueBytes = append(valueBytes, repeat...)

    result := make([]byte, len(valueBytes))

    //加密
    if iv == "" {
        temp := result
        for len(valueBytes) > 0 {
            block.Encrypt(temp, valueBytes[:blocksize])
            valueBytes = valueBytes[blocksize:]
            temp = temp[blocksize:]
        }
    } else {
        //向量切片
        ivBytes := make([]byte, aes.BlockSize)
        copy(ivBytes, []byte(iv))

        encrypter := cipher.NewCBCEncrypter(block, ivBytes)
        encrypter.CryptBlocks(result, valueBytes)
    }

    //以hex格式数值输出
    encryptText := fmt.Sprintf("%x", result)
    return encryptText, nil
}

//AES解密
//iv为空则采用ECB模式，否则采用CBC模式
func aesDecrypt(value, secretKey, iv string) (string, error) {
    if value == "" {
        return "", nil
    }

    //根据秘钥生成8位的秘钥切片
    keyBytes := make([]byte, aes.BlockSize)
    copy(keyBytes, []byte(secretKey))
    //获取block
    block, err := aes.NewCipher(keyBytes)
    if err != nil {
        return "", err
    }

    //将hex格式数据转换为byte切片
    valueBytes := []byte(value)
    var encryptedData = make([]byte, len(valueBytes)/2)
    for i := 0; i < len(encryptedData); i++ {
        b, err := strconv.ParseInt(value[i*2:i*2+2], 16, 10)
        if err != nil {
            return "", err
        }
        encryptedData[i] = byte(b)
    }

    result := make([]byte, len(encryptedData))

    if iv == "" {
        blocksize := block.BlockSize()
        temp := result
        for len(encryptedData) > 0 {
            block.Decrypt(temp, encryptedData[:blocksize])
            encryptedData = encryptedData[blocksize:]
            temp = temp[blocksize:]
        }
    } else {
        //向量切片
        ivBytes := make([]byte, aes.BlockSize)
        copy(ivBytes, []byte(iv))

        //解密
        blockMode := cipher.NewCBCDecrypter(block, ivBytes)
        blockMode.CryptBlocks(result, encryptedData)
    }

    //取消填充
    unpadding := int(result[len(result)-1])
    result = result[:(len(result) - unpadding)]
    return string(result), nil
}