<?php
require 'config.php';
/*
Demo API Module for Silver Monkey web interface
Created by Gerolkae

Sample Data packet:
Action=[Action]   (Get,Delete,Set)
Section=[section]
Key={key]
*Value=[Value]

*/
/*

*/
if ( (isset($_POST["password"])) && ($_POST["password"]=="$BotPwd") && isset($_POST["user"]) && ($_POST["user"]=="$BotUser")) {

$Section = $_POST["Section"];
$Key = $_POST["Key"];
$Value = $_POST["Value"];

switch ($_POST["action"])
{
case "Get":
echo "v=1";
echo "s=0";
// Status Code.. return non 0 for  error

	break;
case "Set":
echo "v=1";
echo "s=0";
// Status Code.. return non 0 for  error
echo "Set Data";

	break;
case "Delete":
echo "v=1";
echo "s=0";
// Status Code.. return non 0 for  error

 

	break;
default:

}
}
else
{
die("Authentication failure.");
}
?>
