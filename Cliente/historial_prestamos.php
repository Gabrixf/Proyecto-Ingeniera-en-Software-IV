<?php
session_start();
require_once '../conexion.php';

if (!isset($_SESSION['idUsuario'])) {
    header("Location: ../login.php");
    exit();
}

try {
    $conn = Conexion::ConexionBD();
    $idUsuario = $_SESSION['idUsuario'];

    // Buscar el IdCliente basado en el IdUsuario
    $stmtCliente = $conn->prepare("SELECT IdCliente FROM Clientes WHERE IdUsuario = ?");
    $stmtCliente->execute([$idUsuario]);
    $cliente = $stmtCliente->fetch(PDO::FETCH_ASSOC);

    if (!$cliente) {
        $_SESSION['mensaje'] = "Cliente no encontrado.";
        $_SESSION['tipo'] = "danger";
        header('Location: login.php');
        exit();
    }

    $idCliente = $cliente['IdCliente'];

    // Consultar el historial de préstamos de ese cliente
    $stmtPrestamos = $conn->prepare("SELECT MontoPrestamo, PlazoMeses, FechaInicio, Estado FROM Prestamos WHERE IdCliente = ?");
    $stmtPrestamos->execute([$idCliente]);
    $prestamos = $stmtPrestamos->fetchAll(PDO::FETCH_ASSOC);

} catch (PDOException $e) {
    $_SESSION['mensaje'] = "Error al cargar historial: " . $e->getMessage();
    $_SESSION['tipo'] = "danger";
    header('Location: login.php');
    exit();
}
?>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>Historial de Préstamos</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="bg-light">

<div class="container mt-5">
    <h2 class="text-center mb-4">Historial de Préstamos</h2>

    <?php if (isset($_SESSION['mensaje'])): ?>
        <div class="alert alert-<?= $_SESSION['tipo'] ?>">
            <?= $_SESSION['mensaje'] ?>
        </div>
        <?php unset($_SESSION['mensaje'], $_SESSION['tipo']); ?>
    <?php endif; ?>

    <?php if (!empty($prestamos)): ?>
        <div class="table-responsive">
            <table class="table table-bordered table-hover">
                <thead class="table-dark">
                    <tr>
                        <th>Monto</th>
                        <th>Plazo (meses)</th>
                        <th>Fecha de inicio</th>
                        <th>Estado</th>
                    </tr>
                </thead>
                <tbody>
                    <?php foreach ($prestamos as $prestamo): ?>
                        <tr>
                            <td>₡<?= number_format($prestamo['MontoPrestamo'], 2) ?></td>
                            <td><?= htmlspecialchars($prestamo['PlazoMeses']) ?></td>
                            <td><?= htmlspecialchars(date('d/m/Y', strtotime($prestamo['FechaInicio']))) ?></td>
                            <td><?= htmlspecialchars($prestamo['Estado']) ?></td>
                        </tr>
                    <?php endforeach; ?>
                </tbody>
            </table>
        </div>
    <?php else: ?>
        <div class="alert alert-info text-center">
            No tenés préstamos registrados aún.
        </div>
    <?php endif; ?>
</div>

</body>
</html>
