<?php
session_start();
require_once '../conexion.php';

if (!isset($_SESSION['idUsuario'])) {
    header("Location: ../login.php");
    exit();
}

$conn = Conexion::ConexionBD();
$idUsuario = $_SESSION['idUsuario'];

// Buscar el IdCliente relacionado al usuario
$stmt = $conn->prepare("SELECT IdCliente FROM Clientes WHERE IdUsuario = ?");
$stmt->execute([$idUsuario]);
$cliente = $stmt->fetch(PDO::FETCH_ASSOC);

if (!$cliente) {
    $_SESSION['mensaje'] = "No se encontró información del cliente.";
    $_SESSION['tipo'] = "danger";
    header('Location: ../login.php');
    exit();
}

$idCliente = $cliente['IdCliente'];

// 1. Total préstamos solicitados
$totalPrestamos = $conn->query("SELECT COUNT(*) FROM Prestamos WHERE IdCliente = $idCliente")->fetchColumn();

// 2. Préstamos pendientes
$prestamosPendientes = $conn->query("SELECT COUNT(*) FROM Prestamos WHERE IdCliente = $idCliente AND Estado = 'Pendiente'")->fetchColumn();

// 3. Préstamos aprobados
$prestamosAprobados = $conn->query("SELECT COUNT(*) FROM Prestamos WHERE IdCliente = $idCliente AND Estado = 'Aprobado'")->fetchColumn();

// 4. Deuda actual
$deudaTotal = $conn->query("SELECT SUM(SaldoPendiente) FROM Prestamos WHERE IdCliente = $idCliente AND Estado = 'Aprobado'")->fetchColumn();
$deudaTotal = $deudaTotal ?: 0;




?>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>Dashboard Cliente</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="bg-light">

<div class="container mt-5">
    <h2 class="text-center mb-4">¡Bienvenido, <?= $_SESSION['nombre'] ?? 'Cliente' ?>!</h2>

    <div class="row g-4">
        <div class="col-md-4">
            <div class="card shadow text-center">
                <div class="card-body">
                    <h5 class="card-title">Total Préstamos</h5>
                    <p class="card-text display-6"><?= $totalPrestamos ?></p>
                </div>
            </div>
        </div>
        
        <div class="col-md-4">
            <div class="card shadow text-center">
                <div class="card-body">
                    <h5 class="card-title">Pendientes</h5>
                    <p class="card-text display-6 text-warning"><?= $prestamosPendientes ?></p>
                </div>
            </div>
        </div>

        <div class="col-md-4">
            <div class="card shadow text-center">
                <div class="card-body">
                    <h5 class="card-title">Aprobados</h5>
                    <p class="card-text display-6 text-success"><?= $prestamosAprobados ?></p>
                </div>
            </div>
        </div>

        <div class="col-md-6 mt-4">
            <div class="card shadow text-center">
                <div class="card-body">
                    <h5 class="card-title">Deuda Actual (₡)</h5>
                    <p class="card-text display-5 text-danger"><?= number_format($deudaTotal, 2) ?></p>
                </div>
            </div>
        </div>

    
    </div>

    <div class="text-center mt-4">
        <a href="solicitar_prestamo.php" class="btn btn-primary">Solicitar un nuevo préstamo</a>
    </div>
    <div class="text-center mt-4">
        <a href="historial_prestamos.php" class="btn btn-primary">Historial de prestamos</a>
    </div>
</div>

</body>
</html>