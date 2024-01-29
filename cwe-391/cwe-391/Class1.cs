using Accenture.OCR.CloudServices.BA;
using Accenture.OCR.CloudServices.DA.Model;
using Accenture.OCR.CloudServices.Models;
using DocumentFormat.OpenXml.Drawing.Charts;
using Newtonsoft.Json;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XXXXXXXX
{
    public class ML
    {
        ModuleSetup<MLExtractConfig> Configuration = new ModuleSetup<MLExtractConfig>();
        ReceviedRequestLogic ReceivedRequest = new ReceviedRequestLogic();
        public ML()
        {

        }
        public ML(ModuleSetup<MLExtractConfig> config)
        {
            Configuration = config;
        }
        public APIResult GetMLResponse(MLInput Input, string Connection, string AppKey)
        {
            string CurrentRequestId = string.Empty;
            APIResult APIOutput = new APIResult();
            try
            {
                string RequestJson = JsonConvert.SerializeObject(Input);

                dynamic requestbody = new ExpandoObject();
                requestbody.FileName = Input.FileName;
                requestbody.FileId = Input.RequestId;
                requestbody.content = Input.Content;

                // Inserting New Request in the Database 
                CurrentRequestId = ReceivedRequest.InsertUpdateReceivedDocument(Connection, "New", "", Configuration.ServiceModuleMapId, Input.FileName, new FileInfo(Input.FileName).Extension, Input.Content,
                    JsonConvert.SerializeObject(requestbody), "", "", -1, -1, -1, Guid.Parse(AppKey));

                string URL = Configuration.ModuleConfiguration.UriBase;
                var client = new RestClient(URL);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Cookie", "BIGipServeratcatnwa.accenture.com-443=!9C0DK7LnfHh4tmrla2hE821J4rnY3i9wt0MOf9Gq3BA+JQMzvuWB5jopVZM4LkqokGYjdAdbcpTRvQw=");
                request.AddParameter("application/json", RequestJson, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                string Result = string.Empty;
                if (response.StatusCode.ToString() != "InternalServerError")
                {
                    Result = response.Content;
                    CurrentRequestId = ReceivedRequest.InsertUpdateReceivedDocument(Connection, "In Progress", CurrentRequestId, -1, "", "", Array.Empty<byte>(),
                                                                 "", "", response.Content, 1, 1, -1, Guid.Parse(AppKey));
                    APIOutput.Result = Result;
                }
                else
                {
                    CurrentRequestId = ReceivedRequest.InsertUpdateReceivedDocument(Connection, "Failed", CurrentRequestId, -1, "", "", Array.Empty<byte>(),
                                                            "", "Processing Failed", "", -1, -1, -1, Guid.Parse(AppKey));
                    APIOutput.Result = "";
                }
                APIOutput.RequestId = CurrentRequestId;
                return APIOutput;
            }
            catch (Exception ex)
            {
                CurrentRequestId = ReceivedRequest.InsertUpdateReceivedDocument(Connection, "Failed", CurrentRequestId, -1, "", "", Array.Empty<byte>(),
                                                            "", "Processing Failed" + ex.Message, "", -1, -1, -1, Guid.Parse(AppKey));
                APIOutput.RequestId = CurrentRequestId;
                APIOutput.Result = "";
                return APIOutput;
            }
        }

        public static FieldExtractionOutput GetResultlFromMLOUtput(MLOutput ML)
        {
            FieldExtractionOutput Output = new FieldExtractionOutput();
            try
            {
                Output.RequestID = Guid.NewGuid().ToString();
                Output.InputRequestID = ML.EntityExtraction.RequestId;

                Output.FullOCR = null;
                Output.ContentLocation = null;

                List<Models.KeyValuePair> lKVPair = new List<Models.KeyValuePair>();
                List<LineItem> lList = new List<LineItem>();
                foreach (Entity e in ML.EntityExtraction.Entity)
                {
                    if (e.Type.ToLower() == "h")
                    {
                        Models.KeyValuePair kvPair = new Models.KeyValuePair();
                        kvPair.FieldName = e.FieldName;
                        List<KeyValue> Ext = new List<KeyValue>();

                        foreach (EntityCapture EC in e.EntityCapture)
                        {
                            try
                            {

                                KeyValue kV = new KeyValue();
                                //Value v = EC.Value;
                                kV.FieldText = EC.Value;
                                kV.FieldValue = EC.Value;
                                kV.Location = EC.Coords;

                                kV.PageNo = Convert.ToInt32(EC.PageNumber);
                                kV.Confidence = Math.Ceiling(Convert.ToDecimal(EC.ConfidenceLevel)).ToString();
                                Ext.Add(kV);
                            }
                            catch (Exception ex)
                            {
                                KeyValue kV = new KeyValue();
                                kV.FieldText = "";
                                kV.FieldValue = "";
                                kV.Location = "";

                                kV.PageNo = 0;
                                kV.Confidence = "0";
                                Ext.Add(kV);
                            }
                        }
                        kvPair.Extraction = Ext;
                        lKVPair.Add(kvPair);
                        Output.FieldOutput = lKVPair;

                    }
                    else
                    {

                        LineItem lineItem = new LineItem();
                        lineItem.FieldName = e.FieldName;

                        foreach (EntityCapture EC in e.EntityCapture)
                        {
                            try
                            {
                                LineValue lValue = new LineValue();
                                lValue.ItemText = EC.Value;
                                lValue.ItemValue = EC.Value;
                                //lValue.Confidence = Math.Ceiling(EC.ConfidenceLevel).ToString();
                                lValue.Confidence = EC.ConfidenceLevel;
                                lValue.Page = Convert.ToInt32(EC.PageNumber);
                                lValue.Location = EC.Coords;
                                lineItem.LineNumber = Convert.ToInt32(EC.RowNo);
                                List<LineValue> list = new List<LineValue>();
                                list.Add(lValue);
                                lineItem.LineOutput = list;
                            }
                            catch (Exception ex)
                            {
                                LineValue lValue = new LineValue();
                                lValue.ItemText = "";
                                lValue.ItemValue = "";
                                lValue.Confidence = "0";
                                lValue.Page = 0;
                                lValue.Location = "";
                                lineItem.LineNumber = 1;
                                List<LineValue> list = new List<LineValue>();
                                list.Add(lValue);
                                lineItem.LineOutput = list;
                            }

                            lList.Add(lineItem);
                            break;
                        }


                        Output.TableData = lList;
                    }
                }

                return Output;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GetFinancialDocResponse(MLFinancialDocInput Input, string Connection, string AppKey)
        {
            string CurrentRequestId = string.Empty;
            try
            {
                string RequestJson = JsonConvert.SerializeObject(Input);

                dynamic requestbody = new ExpandoObject();
                requestbody.FileName = Input.Document[0].DocumentName;
                requestbody.FileId = Input.Document[0].RecievedDocumentId;
                requestbody.content = Input.Content;

                // Inserting New Request in the Database 
                CurrentRequestId = ReceivedRequest.InsertUpdateReceivedDocument(Connection, "New", "", Configuration.ServiceModuleMapId, Input.Document[0].RecievedDocumentId, new FileInfo(Input.Document[0].DocumentName).Extension, Input.Content,
                    JsonConvert.SerializeObject(requestbody), "", "", -1, -1, -1, Guid.Parse(AppKey));

                string URL = Configuration.ModuleConfiguration.UriBase;
                var client = new RestClient(URL);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Cookie", "BIGipServeratcatnwa.accenture.com-443=!9C0DK7LnfHh4tmrla2hE821J4rnY3i9wt0MOf9Gq3BA+JQMzvuWB5jopVZM4LkqokGYjdAdbcpTRvQw=");
                request.AddParameter("application/json", RequestJson, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                string Result = string.Empty;
                if (response.StatusCode.ToString() != "InternalServerError")
                {
                    Result = response.Content;
                    CurrentRequestId = ReceivedRequest.InsertUpdateReceivedDocument(Connection, "In Progress", CurrentRequestId, -1, "", "", Array.Empty<byte>(),
                                                                 "", "", response.Content, 1, 1, -1, Guid.Parse(AppKey));

                }
                else
                {
                    CurrentRequestId = ReceivedRequest.InsertUpdateReceivedDocument(Connection, "Failed", CurrentRequestId, -1, "", "", Array.Empty<byte>(),
                                                            "", "Processing Failed", "", -1, -1, -1, Guid.Parse(AppKey));
                }
                //Console.WriteLine(response.Content);
                return $"{Result}||||{CurrentRequestId}";
            }
            catch (Exception ex)
            {
                CurrentRequestId = ReceivedRequest.InsertUpdateReceivedDocument(Connection, "Failed", CurrentRequestId, -1, "", "", Array.Empty<byte>(),
                                                            "", "Processing Failed" + ex.Message, "", -1, -1, -1, Guid.Parse(AppKey));
                return ex.Message;
            }
        }

        public APIResult FullOCRExtract(string RequestID, string FileName, byte[] FileData, string ClientConnStr, string Appkey, string OcrId = "")
        {
            string CurrentRequestId = string.Empty;
            APIResult APIOutput = new APIResult();
            try
            {
                ContentModel contentModel = new ContentModel()
                {
                    Content = FileData
                };

                string RequestJson = JsonConvert.SerializeObject(contentModel);

                dynamic requestbody = new ExpandoObject();
                requestbody.FileName = FileName;
                requestbody.FileId = RequestID;
                requestbody.content = FileData;

                // Inserting New Request in the Database 
                CurrentRequestId = ReceivedRequest.InsertUpdateReceivedDocument(ClientConnStr, "New", OcrId, Configuration.ServiceModuleMapId, FileName, new FileInfo(FileName).Extension, FileData,
                    JsonConvert.SerializeObject(requestbody), "", "", -1, -1, -1, Guid.Parse(Appkey));

                string URL = Configuration.ModuleConfiguration.UriBase;
                var client = new RestClient(URL);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", RequestJson, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                string Result = string.Empty;
                if (response.StatusCode.ToString() != "InternalServerError")
                {
                    Result = response.Content;
                    CurrentRequestId = ReceivedRequest.InsertUpdateReceivedDocument(ClientConnStr, "In Progress", CurrentRequestId, -1, "", "", Array.Empty<byte>(),
                                                                 "", "", response.Content, 1, 1, -1, Guid.Parse(Appkey));
                    OCRData data = new OCRData();
                    List<FullOCR> ocrs = new List<FullOCR>();
                    CoordResponse coordResponse = JsonConvert.DeserializeObject<CoordResponse>(Result);
                    foreach (PDFOUTPUT pdfOutput in coordResponse.PDFOutput)
                    {
                        int height = Convert.ToInt32(Convert.ToInt32(pdfOutput.measureddimension.Split('X')[1]) * 1.333333333333);
                        int width = Convert.ToInt32(Convert.ToInt32(pdfOutput.measureddimension.Split('X')[0]) * 1.333333333333);

                        FullOCR fullOCR = new FullOCR();
                        fullOCR.PageNumber = pdfOutput.page_no;
                        fullOCR.ContentType = "PDF";
                        fullOCR.ImageData = Convert.FromBase64String(pdfOutput.ImageData);
                        fullOCR.MeasuredDimension = width + "X" + height;

                        List<OCRBoundingBox> boxes = new List<OCRBoundingBox>();
                        pdfOutput.OCROutput?.ForEach(O =>
                        {
                            string Coords = string.Join(",", O.coordinates);
                            boxes.Add(new OCRBoundingBox()
                            {
                                Word = O.word,
                                CoOrdinates = Coords,
                                Confidence = ""
                            });
                        });
                        fullOCR.BoundingBoxes = boxes;
                        new ContentsLogic().InsertUpdateFullOCR(ClientConnStr, Guid.Parse(CurrentRequestId), Guid.Parse(Appkey), JsonConvert.SerializeObject(fullOCR), fullOCR.PageNumber);
                        ocrs.Add(fullOCR);
                    }
                    data.FullOCRPages = ocrs;

                    APIOutput.RequestId = CurrentRequestId;
                    APIOutput.Result = JsonConvert.SerializeObject(data);
                }
                else
                {
                    CurrentRequestId = ReceivedRequest.InsertUpdateReceivedDocument(ClientConnStr, "Failed", CurrentRequestId, -1, "", "", Array.Empty<byte>(),
                                                            "", "Processing Failed", "", -1, -1, -1, Guid.Parse(Appkey));
                    APIOutput.RequestId = CurrentRequestId;
                    APIOutput.Result = "";
                }
                return APIOutput;
            }
            catch (Exception ex)
            {
                CurrentRequestId = ReceivedRequest.InsertUpdateReceivedDocument(ClientConnStr, "Failed", CurrentRequestId, -1, "", "", Array.Empty<byte>(),
                                                            "", "Processing Failed -" + ex.Message, "", -1, -1, -1, Guid.Parse(Appkey));
                APIOutput.RequestId = CurrentRequestId;
                APIOutput.Result = "";
                return APIOutput;
            }
        }

        public APIResult XMLExtract(string RequestID, string FileName, byte[] FileData, string ClientConnStr, string Appkey, string OcrId = "")
        {
            string CurrentRequestId = string.Empty;
            APIResult APIOutput = new APIResult();
            try
            {
                ContentModel contentModel = new ContentModel()
                {
                    FileName = FileName,
                    Content = FileData
                };

                string RequestJson = JsonConvert.SerializeObject(contentModel);

                dynamic requestbody = new ExpandoObject();
                requestbody.FileName = FileName;
                requestbody.FileId = RequestID;
                requestbody.content = FileData;

                // Inserting New Request in the Database 
                CurrentRequestId = ReceivedRequest.InsertUpdateReceivedDocument(ClientConnStr, "New", OcrId, Configuration.ServiceModuleMapId, FileName, new FileInfo(FileName).Extension, FileData,
                    JsonConvert.SerializeObject(requestbody), "", "", -1, -1, -1, Guid.Parse(Appkey));

                string URL = Configuration.ModuleConfiguration.UriBase;
                var client = new RestClient(URL);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", RequestJson, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                string Result = string.Empty;
                if (response.StatusCode.ToString() != "InternalServerError")
                {
                    Result = response.Content;
                    CurrentRequestId = ReceivedRequest.InsertUpdateReceivedDocument(ClientConnStr, "In Progress", CurrentRequestId, -1, "", "", Array.Empty<byte>(),
                                                                 "", "", response.Content, 1, 1, -1, Guid.Parse(Appkey));

                    DataSet dsXML = new DataSet();
                    string xmlConfig = Configuration.ModuleConfiguration.XML;
                    byte[] byteArray = Encoding.UTF8.GetBytes(xmlConfig);
                    MemoryStream stream = new MemoryStream(byteArray);
                    dsXML.ReadXmlSchema(stream);

                    int count = 1;
                    int desCount = 1;
                    int blockCount = 1;
                    int lnCount = 1;
                    double inchheight = 0, inchwidth = 0;

                    MemoryStream pstream = new MemoryStream(FileData);

                    dsXML.Tables["document"].Rows.Add("XMlFromML", "1");
                    CoordResponse coordResponse = JsonConvert.DeserializeObject<CoordResponse>(Result);
                    coordResponse.PDFOutput = Tokenization(coordResponse.PDFOutput);
                    foreach (PDFOUTPUT pdfOutput in coordResponse.PDFOutput)
                    {
                        try
                        {
                            using (PdfDocument pdf = PdfReader.Open(pstream))
                            {
                                XGraphics xGraphics = XGraphics.FromPdfPage(pdf.Pages[count - 1]);
                                inchheight = (xGraphics.PdfPage.Height.Inch);
                                inchwidth = (xGraphics.PdfPage.Width.Inch);
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        double height = Convert.ToDouble(pdfOutput.measureddimension.Split('X')[1]);
                        double width = Convert.ToDouble(pdfOutput.measureddimension.Split('X')[0]);

                        dsXML.Tables["page"].Rows.Add(count, "ML", "Cloud", "1");
                        dsXML.Tables["description"].Rows.Add(desCount, "", "", count.ToString());

                        //int dpiX = 300;
                        //int dpiY = 300;

                        int pixelheight = Convert.ToInt32(height * 1.333333333333);
                        int pixelwidth = Convert.ToInt32(width * 1.333333333333);

                        int dpiX = Convert.ToInt32(pixelwidth / inchwidth);
                        int dpiY = Convert.ToInt32(pixelheight / inchheight);

                        dsXML.Tables["source"].Rows.Add(FileName, dpiX.ToString(), dpiY.ToString(), width.ToString(), height.ToString(), count.ToString());
                        dsXML.Tables["theoreticalPage"].Rows.Add("size", "0", "0", "0", "0", width.ToString(), height.ToString(), count.ToString());
                        dsXML.Tables["zones"].Rows.Add(count, count);
                        dsXML.Tables["textZone"].Rows.Add(blockCount.ToString(), "", "", "", "", "", "", "", "0", count);
                        dsXML.Tables["ln"].Rows.Add(lnCount.ToString(), "", "", "", "", "", "", "", blockCount.ToString());

                        foreach (OCROutput boxes in pdfOutput.OCROutput)
                        {
                            if (!string.IsNullOrEmpty(boxes.word))
                                dsXML.Tables["wd"].Rows.Add(Convert.ToInt32(boxes.coordinates[0] * 20), Convert.ToInt32(boxes.coordinates[1] * 20), Convert.ToInt32(boxes.coordinates[2] * 20), Convert.ToInt32(boxes.coordinates[3] * 20), "", "", boxes.word, lnCount.ToString());
                        }

                        lnCount++;
                        blockCount++;
                        desCount++;
                        count++;
                    }

                    string outputXML = dsXML.GetXml();

                    CurrentRequestId = new ReceviedRequestLogic().InsertUpdateReceivedDocument(ClientConnStr, "In Progress", CurrentRequestId, -1, "", "", Array.Empty<byte>(),
                                                                "", "", outputXML, 1, 1, -1, Guid.Parse(Appkey));

                    APIOutput.RequestId = CurrentRequestId;
                    APIOutput.Result = outputXML;
                }
                else
                {
                    CurrentRequestId = ReceivedRequest.InsertUpdateReceivedDocument(ClientConnStr, "Failed", CurrentRequestId, -1, "", "", Array.Empty<byte>(),
                                                            "", "Processing Failed", "", -1, -1, -1, Guid.Parse(Appkey));
                    APIOutput.RequestId = CurrentRequestId;
                    APIOutput.Result = "";
                }
                return APIOutput;
            }
            catch (Exception ex)
            {
                CurrentRequestId = ReceivedRequest.InsertUpdateReceivedDocument(ClientConnStr, "Failed", CurrentRequestId, -1, "", "", Array.Empty<byte>(),
                                                            "", "Processing Failed -" + ex.Message, "", -1, -1, -1, Guid.Parse(Appkey));
                APIOutput.RequestId = CurrentRequestId;
                APIOutput.Result = "";
                return APIOutput;
            }
        }

        public static List<PDFOUTPUT> Tokenization(List<PDFOUTPUT> PDFOutput)
        {
            List<PDFOUTPUT> PDFOUTPUTlist = new List<PDFOUTPUT>();
            try
            {
                foreach (var item in PDFOutput)
                {
                    PDFOUTPUT pDFOUTPUT = new PDFOUTPUT();
                    pDFOUTPUT.measureddimension = item.measureddimension;
                    pDFOUTPUT.page_no = item.page_no;
                    pDFOUTPUT.ImageData = item.ImageData;
                    List<OCROutput> ocrOutputList = new List<OCROutput>();
                    // bind ocr output 
                    foreach (var a in item.OCROutput)
                    {
                        string[] splittedword = a.word.Split(" ");

                        for (int i = 0; i < splittedword.Length; i++)
                        {
                            OCROutput oCROutput = new OCROutput();
                            oCROutput.coordinates = a.coordinates;
                            oCROutput.page_no = a.page_no;
                            oCROutput.word = splittedword[i].ToString();
                            ocrOutputList.Add(oCROutput);
                        }

                    }
                    item.OCROutput = ocrOutputList;
                    pDFOUTPUT.OCROutput = item.OCROutput;
                    PDFOUTPUTlist.Add(pDFOUTPUT);
                }
                return PDFOUTPUTlist;
            }
            catch (Exception ex)
            {
                return PDFOUTPUTlist;
            }
        }
    }
}
