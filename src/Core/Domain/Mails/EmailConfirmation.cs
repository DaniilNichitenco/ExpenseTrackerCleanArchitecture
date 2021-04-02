using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Domain.Mails
{
    public static class EmailConfirmation
    {
        public const string Message = "<table width=\"100%\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0; width: 100%;\" class=\"background\">" +
  "<tr />" +
    "<td align =\"center\" valign=\"top\" style=\"border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0;\" bgcolor=\"#ffffff\">" +
        "<table border =\"0\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" width=\"600\" style=\"border-collapse: collapse; border-spacing: 0; padding: 0; max-width: 600px;\"\" class=\"wrapper\">" +
            "<tr>"+
              "<td align =\"left\" style=\"font-family: sans-serif; padding:20px; color:#999999; font-size: 12px;\">" +
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit.Donec faucibus enim eget iaculis eleifend.Suspendisse bibendum porttitor gravida." +
          "</td>" +
          "<td align =\"right\" style=\"font-family: sans-serif; padding:20px;\">" +
            "<a target =\"_blank\" style=\"text-decoration: none;\" href=\"#\">" +
              "<img border =\"0\" vspace=\"0\" hspace=\"0\" src=\"https://assets.codepen.io/238794/logo-email.svg\" width=\"200\" height=\"100\" alt=\"Logo\" title=\"Logo\" />" +
            "</a>" +
          "</td>" +
        "</tr>" +
      "</table>" +
    "</td>" +
  "</tr>" +
  "<tr>" +
    "<td align =\"center\" valign=\"top\" style=\"border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0;\">" +
      "<table bgcolor =\"#541f5f\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" width=\"600\" style=\"border-collapse: collapse; border-spacing: 0; padding: 0; max-width: 600px;\" class=\"wrapper\">" +
        "<tr>" +
          "<td align =\"center\" valign=\"top\" style=\"border-collapse: collapse; border-spacing: 0; margin: 0; padding: 40px; font-size: 28px; font-weight: bold; color: #ffffff; font-family: sans-serif;\" class=\"header\">" +
             "[HEADING]" +
         "</td>" +
        "</tr>" +
       "</table>" +
     "</td>" +
   "</tr>" +
  "<tr>" +
     "<td align =\"center\" valign=\"top\" style=\"border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0; padding-top: 5px;\" bgcolor=\"#FFFFFF\">" +
       "<table border =\"0\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" width=\"600\" style=\"border-collapse: collapse; border-spacing: 0; padding: 0; max-width: 600px;\" class=\"wrapper\">" +
         "<tr>" +
           "<td style =\"border-collapse: collapse; border-spacing: 0; margin: 0; padding: 20px; font-family: sans-serif;\">" +
               "<h2 style =\"margin:0; padding:0;\">[SUBHEADING]</h2>" +
               "<p>[BODY]</p>"+
           "</td>"+
         "</tr>"+
       "</table>"+
     "</td>"+
  "</tr>"+
  "<tr>"+
    "<td align =\"center\" valign=\"top\" style=\"border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0;\">" +
      "<table bgcolor =\"#541f5f\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" width=\"600\" style=\"border-collapse: collapse; border-spacing: 0; padding: 0; max-width: 600px;\" class=\"wrapper\">" +
        "<tr>" +
          "<td align =\"center\" style=\"border-collapse: collapse; border-spacing: 0; margin: 0; padding: 20px; color: #ffffff; font-family: sans-serif;\">" +
"<p>[FOOTER]</p>"+
            "</ td >" +
                "</ tr >" +
              "</ table >" +
          "</ td > " +
      "</tr>"+
       "<tr>" +
          "<td align =\"left\" valign=\"top\" style=\"border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0;\" class=\"footer\">" +
          "<table bgcolor =\"#ffffff\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" width=\"600\" style=\"border-collapse: collapse; border-spacing: 0; padding: 0; max-width: 600px;\" class=\"wrapper\">"+
              "<tr>" +
                "<td align =\"left\" style=\"font-family: sans-serif; padding:20px; font-size: 13px; font-family: sans-serif;\">" +
                    "<p>[SUBFOOTER]</ p>" +
                  "</td>" +
                  "<td align =\"right\" style=\"font-family: sans-serif; padding:20px; font-size: 13px; font-family: sans-serif;\">" +
                      "<p> All rights reserved</p>" +
                       "</td>"+
        "</tr>"+
      "</table>"+
		"</td>"+
	"</tr>"+
"</table>";
    }
}
