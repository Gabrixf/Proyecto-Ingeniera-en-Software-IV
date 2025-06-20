<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bienvenido a TeBi - Sistema de Préstamos</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <style>
        body {
            background-color: #f8f9fa;
        }
        header {
            background-color: #0d6efd;
            color: white;
            padding: 20px 0;
        }
        footer {
            background-color: #0d6efd;
            color: white;
            text-align: center;
            padding: 10px 0;
            margin-top: 40px;
        }
    </style>
</head>
<body>
<?php

include("header.php");
include("footer.php");
?>

<!-- Header -->


<!-- Contenido principal -->
<div class="container mt-5">
    <div class="row align-items-center">
        <div class="col-md-6">
            <h2>Bienvenido al Sistema de Préstamos TeBi</h2>
            <p class="lead text-muted">Solicitá tu préstamo de manera fácil, segura y 100% en línea.</p>
            <p>Con TeBi podés gestionar tu préstamo sin filas, sin papeleo innecesario y con las mejores condiciones del mercado.</p>
            <div class="mt-4">
                <a href="registro_cliente.php" class="btn btn-primary btn-lg me-2">Registrarse</a>
                <a href="login.php" class="btn btn-outline-primary btn-lg">Iniciar Sesión</a>
            </div>
        </div>
        <div class="col-md-6 text-center">
            <img src="https://img.freepik.com/vector-premium/prestamo-dinero-solicitud-linea-telefono-inteligente_387612-174.jpg" class="img-fluid" alt="Solicitar crédito">
        </div>
    </div>
</div>
