<?php
class Conexion
{
    public static function ConexionBD()
    {
        $host = 'localhost,1433'; // coma, no barra invertida
        $dbname = 'SistemaPrestamos';
        try {
            $conn = new PDO("sqlsrv:Server=$host;Database=$dbname", null, null);
            $conn->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
            return $conn;
        } catch (PDOException $e) {
            die("Error de conexión: " . $e->getMessage());
        }
    }
}
