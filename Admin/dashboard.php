<?php
session_start();
require_once '../conexion.php';

if (!isset($_SESSION['rol']) || $_SESSION['rol'] !== 'Admin') {
    // Puedes mandarlo a login o a un error
    header("Location: ../login.php");
    exit();
}

try {
    $conn = Conexion::ConexionBD();

    // Total de clientes
    $totalClientes = $conn->query("SELECT COUNT(*) AS total FROM Clientes")->fetch(PDO::FETCH_ASSOC)['total'];

    // Total de préstamos activos
    $prestamosActivos = $conn->query("SELECT COUNT(*) AS total FROM Prestamos WHERE Estado = 'Activo'")->fetch(PDO::FETCH_ASSOC)['total'];

    // Total de solicitudes pendientes
    $solicitudesPendientes = $conn->query("SELECT COUNT(*) AS total FROM Prestamos WHERE Estado = 'Pendiente'")->fetch(PDO::FETCH_ASSOC)['total'];

} catch (PDOException $e) {
    die("Error de conexión: " . $e->getMessage());
}
?>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>Dashboard Admin</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="bg-light">

<div class="container mt-5">
    <h1 class="text-center mb-5">Bienvenido al Panel de Administración</h1>

    <div class="row text-center">
        <div class="col-md-4 mb-4">
            <div class="card shadow">
                <div class="card-body">
                    <h5 class="card-title">Clientes Registrados</h5>
                    <p class="display-4"><?= $totalClientes ?></p>
                </div>
            </div>
        </div>
        <div class="col-md-4 mb-4">
            <div class="card shadow">
                <div class="card-body">
                    <h5 class="card-title">Préstamos Activos</h5>
                    <p class="display-4"><?= $prestamosActivos ?></p>
                </div>
            </div>
        </div>
        <div class="col-md-4 mb-4">
            <div class="card shadow">
                <div class="card-body">
                    <h5 class="card-title">Solicitudes Pendientes</h5>
                    <p class="display-4"><?= $solicitudesPendientes ?></p>
                </div>
            </div>
        </div>
    </div>

    <div class="text-center mt-4">
        <a href="ver_solicitudes.php" class="btn btn-primary btn-lg">Ver Solicitudes Pendientes</a>
    </div>

</div>

</body>
</html>