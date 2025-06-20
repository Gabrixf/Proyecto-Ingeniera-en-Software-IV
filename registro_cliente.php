<?php
include 'header.php';
?>

<div class="container mt-5">
    <h3 class="text-center mb-4">Registro de Usuario</h3>

    <?php if (!empty($_SESSION['mensaje'])): ?>
        <div class="alert alert-<?php echo $_SESSION['tipo']; ?>">
            <?php echo $_SESSION['mensaje']; unset($_SESSION['mensaje'], $_SESSION['tipo']); ?>
        </div>
    <?php endif; ?>

    <form action="validar_registro.php" method="POST" class="row g-3">
        <div class="col-md-6">
            <label for="nombre" class="form-label">Nombre</label>
            <input type="text" name="nombre" id="nombre" class="form-control" required>
        </div>

        <div class="col-md-6">
            <label for="apellido" class="form-label">Apellido</label>
            <input type="text" name="apellido" id="apellido" class="form-control" required>
        </div>

        <div class="col-md-6">
            <label for="cedula" class="form-label">Cédula</label>
            <input type="text" name="cedula" id="cedula" class="form-control" required>
        </div>

        <div class="col-md-6">
            <label for="telefono" class="form-label">Teléfono</label>
            <input type="text" name="telefono" id="telefono" class="form-control" required>
        </div>

        <div class="col-md-6">
            <label for="correo" class="form-label">Correo electrónico</label>
            <input type="email" name="correo" id="correo" class="form-control" required>
        </div>

        <div class="col-md-6">
            <label for="contrasena" class="form-label">Contraseña</label>
            <input type="password" name="contrasena" id="contrasena" class="form-control" required>
        </div>

        <div class="col-md-6">
            <label for="ingreso" class="form-label">Ingreso mensual (₡)</label>
            <input type="number" step="0.01" name="ingreso" id="ingreso" class="form-control" required>
        </div>

        <div class="col-12">
            <button type="submit" class="btn btn-primary w-100">Registrarse</button>
        </div>
    </form>
</div>

<?php include 'footer.php'; ?>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>