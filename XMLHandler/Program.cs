using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace XMLHandler
{
    class Program
    {
        public static bool ServiceIsActive;        // флаг для запуска и остановки потока
        public static string AnalyzerResultPath = AppDomain.CurrentDomain.BaseDirectory + "\\Results"; // папка для файлов с результатами
        public static bool FileToErrorPath;        // флаг для перемещения файлов в ошибки или архив

        //принимает на входе код теста, возвращает код услуги в которую входит тест
        public static string BIKCode(string TestCode)
        {
            string InvestigationCode = "";
            switch (TestCode)
            {
                case "Б0035":
                    InvestigationCode = "45.43";
                    break;
                case "Б0005":
                    InvestigationCode = "45.17";
                    break;
                case "Б0060":
                    InvestigationCode = "45.44";
                    break;
                case "Б0065":
                    InvestigationCode = "45.45";
                    break;
                case "Б0040":
                    InvestigationCode = "45.46";
                    break;
                case "Б0001":
                    InvestigationCode = "45.16";
                    break;
                case "Б0025":
                    InvestigationCode = "45.41";
                    break;
                case "Б0030":
                    InvestigationCode = "45.42";
                    break;
                case "Б0050":
                    InvestigationCode = "45.47";
                    break;
                case "Б0140":
                    InvestigationCode = "45.33";
                    break;
                case "Б0135":
                    InvestigationCode = "45.4";
                    break;
                case "Б0105":
                    InvestigationCode = "45.65";
                    break;
                case "Б0131":
                    InvestigationCode = "45.67";
                    break;
                case "Б0130":
                    InvestigationCode = "45.57";
                    break;
                case "ИМ0280":
                    InvestigationCode = "47.19";
                    break;
                case "Б0015":
                    InvestigationCode = "45.29";
                    break;
                case "Б0045":
                    InvestigationCode = "45.48";
                    break;
                case "Б0070":
                    InvestigationCode = "45.50";
                    break;
                case "Б0099":
                    InvestigationCode = "45.38";
                    break;
                case "Б0090":
                    InvestigationCode = "45.39";
                    break;
                case "Б0120":
                    InvestigationCode = "45.58";
                    break;
                case "Б0020":
                    InvestigationCode = "45.31";
                    break;
                case "Б0010":
                    InvestigationCode = "45.28";
                    break;
                case "Б0110":
                    InvestigationCode = "45.66";
                    break;
                case "Б0275":
                    InvestigationCode = "45.22";
                    break;
                case "Б0085":
                    InvestigationCode = "45.36";
                    break;
                case "Б0125":
                    InvestigationCode = "45.60";
                    break;
                case "Б0285":
                    InvestigationCode = "45.68";
                    break;
                case "Б0080":
                    InvestigationCode = "45.37";
                    break;
                case "Б0150":
                    InvestigationCode = "45.6";
                    break;
                case "Б0055":
                    InvestigationCode = "45.51";
                    break;
                case "Б0260":
                    InvestigationCode = "45.24";
                    break;
            }

            return InvestigationCode;

        }

        //ищем атрибут в xml если нет, то возвращает пустую строку
        public static string CheckAttr(XmlNode nodeParam, string attrName)
        {
            try
            {
                string attrValue = nodeParam.Attributes.GetNamedItem(attrName).Value;
                return attrValue;
            }
            catch
            {
                return "";
            }

        }
        public static XDocument CheckXML(XmlDocument InputXML)
        {
            FileToErrorPath = false;
            try
            {
                // получим корневой элемент
                XmlElement xRoot = InputXML.DocumentElement;
                // создаем новый xml документ
                XDocument xDoc = new XDocument();

                #region Envelope
                // Создаем элемент Envelope
                XElement Envelope = new XElement("Envelope");
                // выбор в документе всех узлов с именем "Message", которые находятся в элементах "Envelope"
                XmlNodeList childnodes = xRoot.SelectNodes("//Envelope//Message");
                // обход дочерних узлов Message
                foreach (XmlNode childnode in childnodes)
                {
                    // создаем новый элемент Message, для результирующей xml
                    XElement Message = new XElement("Message");
                    // Создаем аттрибут ID элемента Message
                    XAttribute MessageID = new XAttribute("ID", CheckAttr(childnode, "ID"));
                    // Добавляем аттрибут к новому элементу Message
                    Message.Add(MessageID);
                    XAttribute MessageOrigin = new XAttribute("Origin", CheckAttr(childnode, "Origin"));
                    Message.Add(MessageOrigin);
                    XAttribute MessageType = new XAttribute("Type", CheckAttr(childnode, "Type"));
                    Message.Add(MessageType);
                    XAttribute MessageVersion = new XAttribute("Version", CheckAttr(childnode, "Version"));
                    Message.Add(MessageVersion);
                    //добавляем Message с аттрибутами в Envelope. Этот блок готов к добавлению в результирующую xml
                    Envelope.Add(Message);
                }
                // выбор в документе всех узлов с именем "FromSystem", которые находятся в элементах "Envelope"
                XmlNodeList childnodesFromSystem = xRoot.SelectNodes("//Envelope//FromSystem");
                foreach (XmlNode childnode in childnodesFromSystem)
                {
                    XElement FromSystem = new XElement("FromSystem");
                    XAttribute FromSystemID = new XAttribute("ID", CheckAttr(childnode, "ID"));
                    FromSystem.Add(FromSystemID);
                    Envelope.Add(FromSystem);
                }
                // выбор в документе всех узлов с именем "ToSystem", которые находятся в элементах "Envelope"
                XmlNodeList childnodesToSystemm = xRoot.SelectNodes("//Envelope//ToSystem");
                foreach (XmlNode childnode in childnodesToSystemm)
                {
                    XElement ToSystem = new XElement("ToSystem");
                    XAttribute ToSystemID = new XAttribute("ID", CheckAttr(childnode, "ID"));
                    ToSystem.Add(ToSystemID);
                    Envelope.Add(ToSystem);
                }
                // выбор в документе всех узлов с именем "Sent", которые находятся в элементах "Envelope"
                XmlNodeList childnodesSent = xRoot.SelectNodes("//Envelope//Sent");
                foreach (XmlNode childnode in childnodesSent)
                {
                    XElement Sent = new XElement("Sent");
                    XAttribute SentDateTime = new XAttribute("DateTime", CheckAttr(childnode, "DateTime"));
                    Sent.Add(SentDateTime);
                    Envelope.Add(Sent);
                }
                #endregion

                #region Requistion

                // Создаем элемент Requistion, объявляем вне цикла потому что будет добавляться много элементов
                XElement Requistion = new XElement("Requisition");
                // выбор в документе всех узлов c именем "Requisition"
                XmlNodeList childnodesRequistion = xRoot.SelectNodes("//Requisition");
                // обход дочерних узлов Requisition
                // обработка атрибутов Requistion
                foreach (XmlNode childnode in childnodesRequistion)
                {
                    XAttribute GUID = new XAttribute("GUID", CheckAttr(childnode, "GUID"));
                    Requistion.Add(GUID);
                    XAttribute ReqType = new XAttribute("ReqType", CheckAttr(childnode, "ReqType"));
                    Requistion.Add(ReqType);
                    XAttribute RequistionID = new XAttribute("RequisitionID", CheckAttr(childnode, "RequisitionID"));
                    // Добавляем атрибут к новому элементу Requistion
                    Requistion.Add(RequistionID);
                    XAttribute Status = new XAttribute("Status", CheckAttr(childnode, "Status"));
                    Requistion.Add(Status);
                }

                // элемент ReqInformation
                // выбор в документе всех узлов с именем "ReqInformation", которые находятся в элементах "Requistion"
                XmlNodeList childnodesReqInformation = xRoot.SelectNodes("//Requisition//ReqInformation");
                foreach (XmlNode childnode in childnodesReqInformation)
                {
                    // Создаем элемент ReqInformation, каждый раз объявляем в цикле (это вложенный элемент Requistion)
                    XElement ReqInformation = new XElement("ReqInformation");
                    XAttribute ReqInformationCode = new XAttribute("Code", CheckAttr(childnode, "Code"));
                    ReqInformation.Add(ReqInformationCode);
                    XAttribute ReqInformationValue = new XAttribute("Value", CheckAttr(childnode, "Value"));
                    ReqInformation.Add(ReqInformationValue);

                    // Элемент ReqInformation добавляем в родительский Requistion
                    Requistion.Add(ReqInformation);
                }

                // элемент Patient
                // выбор в документе всех узлов с именем "Patient", которые находятся в элементах "Requistion"
                XmlNodeList childnodesPatient = xRoot.SelectNodes("//Requisition//Patient");
                foreach (XmlNode childnode in childnodesPatient)
                {
                    // создаем элемент Patient
                    XElement Patient = new XElement("Patient");
                    // атрибуты
                    XAttribute BirthDate = new XAttribute("BirthDate", CheckAttr(childnode, "BirthDate"));
                    Patient.Add(BirthDate);
                    XAttribute FirstName = new XAttribute("FirstName", CheckAttr(childnode, "FirstName"));
                    Patient.Add(FirstName);
                    XAttribute Surname = new XAttribute("Surname", CheckAttr(childnode, "Surname"));
                    Patient.Add(Surname);
                    XAttribute PatientPatientID = new XAttribute("PatientID", "");
                    Patient.Add(PatientPatientID);
                    XAttribute ReservePatientID = new XAttribute("ReservePatientID", CheckAttr(childnode, "ReservePatientID"));
                    Patient.Add(ReservePatientID);
                    XAttribute Sex = new XAttribute("Sex", CheckAttr(childnode, "Sex"));
                    Patient.Add(Sex);
                    

                    // Элемент Patient добавляем в родительский Requistion
                    Requistion.Add(Patient);
                }

                /*
                XmlNodeList childnodesICDDiagnosis = xRoot.SelectNodes("//Requisition//ICDDiagnosis");
                foreach (XmlNode childnode in childnodesICDDiagnosis)
                {
                    XElement ICDDiagnosis = new XElement("ICDDiagnosis");
                    XAttribute ICDDiagnosisCode = new XAttribute("Code", CheckAttr(childnode, "Code"));
                    ICDDiagnosis.Add(ICDDiagnosisCode);
                    XAttribute ICDDiagnosisPriority = new XAttribute("Priority", CheckAttr(childnode, "Priority"));
                    ICDDiagnosis.Add(ICDDiagnosisPriority);
                    Requistion.Add(ICDDiagnosis);
                }
                */
                
                
                // элемент ICDDiagnosis
                // выбор в документе всех узлов с именем "ICDDiagnosis", которые находятся в элементах "Requistion"
                XmlNodeList childnodesICDDiagnosis = xRoot.SelectNodes("//Requisition/ICDDiagnosis");
                foreach (XmlNode childnode in childnodesICDDiagnosis)
                {
                    // создаем элемент ICDDiagnosis
                    XElement ICDDiagnosis = new XElement("ICDDiagnosis");
                    // атрибуты
                    XAttribute ICDDiagnosisCode = new XAttribute("Code", CheckAttr(childnode, "Code"));
                    ICDDiagnosis.Add(ICDDiagnosisCode);
                    XAttribute ICDDiagnosisPriority = new XAttribute("Priority", CheckAttr(childnode, "Priority"));
                    ICDDiagnosis.Add(ICDDiagnosisPriority);

                    // Элемент ICDDiagnosis добавляем в родительский Requistion
                    Requistion.Add(ICDDiagnosis);
                }
                

                // элемент ReqUnit
                // выбор в документе всех узлов с именем "ReqUnit", которые находятся в элементах "Requistion"
                XmlNodeList childnodesReqUnit = xRoot.SelectNodes("//Requisition//ReqUnit");
                foreach (XmlNode childnode in childnodesReqUnit)
                {
                    // создаем элемент ReqUnit
                    XElement ReqUnit = new XElement("ReqUnit");
                    // атрибуты элемента
                    XAttribute ContactId = new XAttribute("ContactId", CheckAttr(childnode, "ContactId"));
                    ReqUnit.Add(ContactId);
                    XAttribute ContactPerson = new XAttribute("ContactPerson", CheckAttr(childnode, "ContactPerson"));
                    ReqUnit.Add(ContactPerson);
                    XAttribute PostalAddress = new XAttribute("PostalAddress", CheckAttr(childnode, "PostalAddress"));
                    ReqUnit.Add(PostalAddress);
                    XAttribute AddressCode = new XAttribute("AddressCode", CheckAttr(childnode, "AddressCode"));
                    ReqUnit.Add(AddressCode);

                    // Элемент ReqUnit добавляем в родительский Requistion
                    Requistion.Add(ReqUnit);
                }

                // элемент CopyUnit1
                // выбор в документе всех узлов с именем "CopyUnit1", которые находятся в элементах "Requistion"
                XmlNodeList childnodesCopyUnit1 = xRoot.SelectNodes("//Requisition//CopyUnit1");
                foreach (XmlNode childnode in childnodesCopyUnit1)
                {
                    // создаем элемент CopyUnit1
                    XElement CopyUnit = new XElement("CopyUnit1");
                    // атрибуты элемента
                    XAttribute CopyUnitContactPerson = new XAttribute("ContactPerson", CheckAttr(childnode, "ContactPerson"));
                    CopyUnit.Add(CopyUnitContactPerson);
                    XAttribute CopyUnitAddressCode = new XAttribute("AddressCode", CheckAttr(childnode, "AddressCode"));
                    CopyUnit.Add(CopyUnitAddressCode);

                    // Элемент CopyUnit добавляем в родительский Requistion
                    Requistion.Add(CopyUnit);
                }

                #region Sample
                // элемент Samle
                // выбор в документе всех узлов с именем "CopyUnit1", которые находятся в элементах "Requistion"
                XmlNodeList childnodesSample = xRoot.SelectNodes("//Requisition//Sample");
                // Создаем элемент Sample, в него будут добавляться другие элементы
                XElement Sample = new XElement("Sample");
                // выбор в документе всех узлов с именем "Sample", которые находятся в элементах "Requistion"
                foreach (XmlNode childnode in childnodesSample)
                {
                    // сначала получим аттрибуты sample
                    XAttribute LaboratoryID = new XAttribute("LaboratoryID", CheckAttr(childnode, "LaboratoryID"));
                    Sample.Add(LaboratoryID);
                    XAttribute FormatQualifier = new XAttribute("FormatQualifier", CheckAttr(childnode, "FormatQualifier"));
                    Sample.Add(FormatQualifier);
                    XAttribute DrawTime = new XAttribute("DrawTime", CheckAttr(childnode, "DrawTime"));
                    Sample.Add(DrawTime);

                    // Sample с атрибутами добавляем в Requistion
                    Requistion.Add(Sample);
                }

                // ColUnit
                // выбор в документе всех узлов с именем "ColUnit", которые находятся в элементах "Sample"
                XmlNodeList childnodesColUnit = xRoot.SelectNodes("//Requisition//Sample//ColUnit");
                foreach (XmlNode childnode in childnodesColUnit)
                {
                    // элемент ColUnit
                    XElement ColUnit = new XElement("ColUnit");
                    XAttribute ColUnitAddressCode = new XAttribute("AddressCode", CheckAttr(childnode, "AddressCode"));
                    ColUnit.Add(ColUnitAddressCode);
                    XAttribute ColUnitPartyQualifier = new XAttribute("PartyQualifier", CheckAttr(childnode, "PartyQualifier"));
                    ColUnit.Add(ColUnitPartyQualifier);

                    // ColUnit с атрибутами добавляем в Sample
                    Sample.Add(ColUnit);
                }

                // Investigation
                // выбор в документе всех узлов с именем "Investigation", которые находятся в элементах "Sample"
                XmlNodeList childnodesInvestigation = xRoot.SelectNodes("//Requisition//Sample//Investigation");
                // обход узлов investigation
                foreach (XmlNode childnode in childnodesInvestigation)
                {
                    switch (childnode.Attributes.GetNamedItem("InvestigationCode").Value)
                    {
                        // если услуга = 0.1, то идем по тестам и сопоставляем их с кодом услуги
                        case "0.1":
                            // выбор в документе всех узлов с именем "Analysis", которые находятся в элементах "Sample"
                            XmlNodeList checkAnalysis = xRoot.SelectNodes("//Requisition//Sample//Analysis");
                            foreach (XmlNode analys in checkAnalysis)
                            {
                                string NewInvestigationCode = BIKCode(analys.Attributes.GetNamedItem("TestMethodCode").Value);
                                if (NewInvestigationCode != "")
                                {
                                    // Новый элемент investigation
                                    XElement NewInvestigation = new XElement("Investigation");
                                    // атрибуты
                                    XAttribute NewInvestigationInvCode = new XAttribute("InvestigationCode", NewInvestigationCode);
                                    NewInvestigation.Add(NewInvestigationInvCode);
                                    XAttribute NewInvestigationPrCode = new XAttribute("PriorityCode", analys.Attributes.GetNamedItem("PriorityCode").Value);
                                    NewInvestigation.Add(NewInvestigationPrCode);

                                    // добавляем новый элемент investigation в sample
                                    Sample.Add(NewInvestigation);
                                    
                                }
                            }
                            break;

                        default:
                            XElement Investigation = new XElement("Investigation");
                            XAttribute InvestigationCode = new XAttribute("InvestigationCode", childnode.Attributes.GetNamedItem("InvestigationCode").Value);
                            Investigation.Add(InvestigationCode);
                            XAttribute PriorityCode = new XAttribute("PriorityCode", childnode.Attributes.GetNamedItem("PriorityCode").Value);
                            Investigation.Add(PriorityCode);

                            // добавляем Investigation в Sample
                            Sample.Add(Investigation);
                            break;
                    }
                }

                // Тесты БИК, с ними будем сравнивать текущий тест из Analysys, чтобы подставить услугу, если ее еще нет
                string[] BIKtests = new string[] { "Б0035", "Б0005", "Б0060", "Б0065", "Б0040", "Б0001", "Б0025", "Б0030", "Б0050", "Б0140", 
                                                   "Б0135", "Б0105", "Б0131", "Б0130", "ИМ0280", "Б0015", "Б0045", "Б0070", "Б0099", "Б0090",
                                                   "Б0120", "Б0020", "Б0010", "Б0110", "Б0275", "Б0085", "Б0125", "Б0285", "Б0080", "Б0150", "Б0055", "Б0260" };


                // Analysis
                // выбор в документе всех узлов с именем "Analysis", которые находятся в элементах "Sample"
                XmlNodeList childnodesAnalys = xRoot.SelectNodes("//Requisition//Sample//Analysis");

                #region Добавление услуг для тестов, которые приходят без кода профиля и не в рамках БИК, 0.1 и тд
                foreach (XmlNode childnode in childnodesAnalys)
                {
                    // проверяем тест, относится ли к БИК
                    foreach (string i in BIKtests)
                    {
                        // Если тест из массива БИК соответствует текущему тесту из childnode
                        if (i == childnode.Attributes.GetNamedItem("TestMethodCode").Value)
                        {
                            bool IsInvExist = false; // флаг определяющий, нужно ли добавлять услугу в блок Investigation
                                                     // услуга добавляется только в том случае,когда ее еще нет в Investigation

                            Console.WriteLine($"{childnode.Attributes.GetNamedItem("TestMethodCode").Value} Test from BIK"); // это тест из БИК списка
                            string NewInvCode = BIKCode(childnode.Attributes.GetNamedItem("TestMethodCode").Value);
                            Console.WriteLine(NewInvCode);

                            // Проверка существующих Investigation
                            foreach (XElement inv_el in Sample.Elements("Investigation"))
                            {
                                if (NewInvCode == inv_el.Attribute("InvestigationCode").Value)
                                {
                                    Console.WriteLine("Совпадает код. Такая услуга уже есть.");
                                    IsInvExist = true; // такая услуга есть
                                    Console.WriteLine(IsInvExist);
                                }
                            }

                            if (!IsInvExist)
                            {
                                // создаем новый элемент Investigstion
                                XElement Inv = new XElement("Investigation");
                                XAttribute InvCode = new XAttribute("InvestigationCode", NewInvCode);
                                Inv.Add(InvCode);
                                XAttribute InvPriorityCode = new XAttribute("PriorityCode", childnode.Attributes.GetNamedItem("PriorityCode").Value);
                                Inv.Add(InvPriorityCode);
                                // добавляем Investigation в Sample
                                Sample.Add(Inv);
                            }
                        }
                        /*
                        else
                        {
                            // если это тест из БИК не соответствует текущему тесту, то ничего не делаем
                            //Console.WriteLine($"{childnode.Attributes.GetNamedItem("TestMethodCode").Value} Test NOT from BIK");
                        }
                        */
                    }

                    //Console.WriteLine(TestMethodCode.Value);
                }
                #endregion

                foreach (XmlNode childnode in childnodesAnalys)
                {
                    // Новый элемент Analysis
                    XElement Analysis = new XElement("Analysis");
                    // атрибуты
                    XAttribute AnalysisGroupCode = new XAttribute("GroupCode", "");
                    Analysis.Add(AnalysisGroupCode);
                    XAttribute TestMethodCode = new XAttribute("TestMethodCode", CheckAttr(childnode, "TestMethodCode"));
                    Analysis.Add(TestMethodCode);
                    XAttribute AnalysisPriorityCode = new XAttribute("PriorityCode", CheckAttr(childnode, "PriorityCode"));
                    Analysis.Add(AnalysisPriorityCode);

                    foreach (XmlNode childnodelab in childnode.ChildNodes)
                    {
                        XElement Lab = new XElement("Lab");
                        //childnode.Attributes.GetNamedItem("AdressCode").Value
                        XAttribute LabAdressCode = new XAttribute("AdressCode", CheckAttr(childnodelab, "AdressCode"));
                        Lab.Add(LabAdressCode);
                        Analysis.Add(Lab);
                    }

                    Sample.Add(Analysis);
                }

                #endregion

                #endregion

                // создаем корневой элемент
                XElement SafirMessage = new XElement("SafirMessage");
                // добавляем в корневой элемент
                SafirMessage.Add(Envelope);
                SafirMessage.Add(Requistion);

                // добавляем корневой элемент в документ
                xDoc.Add(SafirMessage);

                //сохраняем документ
                //xDoc.Save("people.xml");

                return xDoc;

            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка XML: " + e);
                FileToErrorPath = true; //флаг указывает на то, что файл будет перемещен в папку с ошибками
                XDocument xDoc = new XDocument();
                return xDoc;
            }

        }

        public static void ResultsProcessing()
        {
            while (ServiceIsActive)
            {
                try
                {
                    if (!Directory.Exists(AnalyzerResultPath))
                    {
                        Directory.CreateDirectory(AnalyzerResultPath);
                    }

                    string ArchivePath = AnalyzerResultPath + @"\Archive";
                    string ErrorPath = AnalyzerResultPath + @"\Error";
                    string CGMPath = AnalyzerResultPath + @"\CGM";

                    if (!Directory.Exists(ArchivePath))
                    {
                        Directory.CreateDirectory(ArchivePath);
                    }

                    if (!Directory.Exists(ErrorPath))
                    {
                        Directory.CreateDirectory(ErrorPath);
                    }

                    if (!Directory.Exists(CGMPath))
                    {
                        Directory.CreateDirectory(CGMPath);
                    }

                    string[] Files = Directory.GetFiles(AnalyzerResultPath, "*.xml");
                    foreach (string file in Files)
                    {

                        string FileName = file.Substring(AnalyzerResultPath.Length + 1);

                        string destFile = System.IO.Path.Combine(CGMPath, FileName);

                        // переменная для входящего файла xml
                        XmlDocument InxDoc = new XmlDocument();
                        InxDoc.Load(file);
                        
                        // формирование новой xml
                        CheckXML(InxDoc).Save(destFile);

                        // Перемещение файлов в архив или ошибки
                        if (!FileToErrorPath)
                        {
                            if (System.IO.File.Exists(ArchivePath + @"\" + FileName))
                            {
                                System.IO.File.Delete(ArchivePath + @"\" + FileName);
                            }
                            System.IO.File.Move(file, ArchivePath + @"\" + FileName);
                            Console.WriteLine("File has been moved to Archive folder.");
                        }
                        else
                        {
                            if (System.IO.File.Exists(ErrorPath + @"\" + FileName))
                            {
                                System.IO.File.Delete(ErrorPath + @"\" + FileName);
                            }
                            System.IO.File.Move(file, ErrorPath + @"\" + FileName);
                            Console.WriteLine("File has been moved to Error folder.");
                            
                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e}");
                }

                //Thread.Sleep(1000);
            }
        }

        static void Main(string[] args)
        {
            ServiceIsActive = true;
            Console.WriteLine("Program XMLHandler starts working.");

            // Поток обработки результатов
            /*
            Thread ResultProcessingThread = new Thread(ResultsProcessing);
            ResultProcessingThread.Name = "ResultsProcessing";
            ResultProcessingThread.Start();
            */
            ResultsProcessing();

            Console.ReadLine();
        }
    }
}
