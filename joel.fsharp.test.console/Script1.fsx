#light

open System.Net
open System.IO
open System.Linq
open System.Xml
open System

let urls = [|
    "http://www.euro-millions.com/results-archive-2013.asp"
    "http://www.euro-millions.com/results-archive-2012.asp"
|]

let fetchRawHtml (url : string) =
    let req = WebRequest.Create(url)
    let resp = req.GetResponse()
    let stream = resp.GetResponseStream()
    let reader = new StreamReader(stream)
    reader.ReadToEnd()

let rawHtml =
    urls
    |> Seq.map (fun url -> fetchRawHtml(url))

let rec fetchResultsFromTable (table : string) =
    let openingOfStartTag = table.IndexOf("<td")

    if openingOfStartTag < 0 then
        []
    else
        let closingOfStartTag = table.IndexOf(">", openingOfStartTag) + 1
        let closingTag = table.IndexOf("</td>", closingOfStartTag)
        table.Substring(closingOfStartTag, closingTag - closingOfStartTag) :: fetchResultsFromTable(table.Substring(closingTag + 4))

let rec fetchResultsFromPage (pageHtml : string) =
        let startPos = pageHtml.IndexOf("<table align=\"right\" class=\"ticketChecker\">")
        
        if startPos < 0 then 
            []
        else 
            let endPos = pageHtml.IndexOf("</table>", startPos) + 8
            let subStringHtml = pageHtml.Substring(endPos)
            fetchResultsFromTable(pageHtml.Substring(startPos, endPos - startPos)) :: fetchResultsFromPage(subStringHtml)

let results = 
    rawHtml
    |> Seq.collect (fun html -> fetchResultsFromPage(html))