const dialogflow = require('dialogflow');
const uuid = require('uuid');

// Thay YOUR_PROJECT_ID bằng ID dự án Google Cloud của bạn
const projectId = 'zeta-resource-454909-a5';
// Tạo session ID ngẫu nhiên
const sessionId = uuid.v4();
const sessionClient = new dialogflow.SessionsClient();
const sessionPath = sessionClient.sessionPath(projectId, sessionId);

/**
 * Hàm gửi văn bản đến Dialogflow và nhận phản hồi.
 * @param {string} text Văn bản truy vấn.
 */
async function detectIntent(text) {
    const request = {
        session: sessionPath,
        queryInput: {
            text: {
                text: text,
                languageCode: 'vi' // Đã thay đổi thành tiếng Việt
            }
        }
    };

    const responses = await sessionClient.detectIntent(request);
    const result = responses[0].queryResult;
    
    console.log('Đã nhận diện ý định');
    console.log(`  Câu hỏi: ${result.queryText}`);
    console.log(`  Phản hồi: ${result.fulfillmentText}`);
    return result;
}

// Ví dụ sử dụng: gửi câu hỏi đến bot
detectIntent("Xin chào, tôi tên là Bob").catch(console.error);