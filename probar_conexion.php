<?php
include("conexion.php");

$conn = Conexion::ConexionBD();

if ($conn) {
    echo "Conexión exitosa a SQL Server";
}
?>